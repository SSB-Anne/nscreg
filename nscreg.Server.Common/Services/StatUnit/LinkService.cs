﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using nscreg.Data;
using nscreg.Data.Constants;
using nscreg.Data.Core;
using nscreg.Data.Entities;
using nscreg.Resources.Languages;
using nscreg.Server.Common.Models.Links;
using nscreg.Server.Common.Models.Lookup;
using nscreg.Server.Common.Models.StatUnits;
using nscreg.Utilities.Enums;
using nscreg.Utilities.Extensions;

namespace nscreg.Server.Common.Services.StatUnit
{
    public class LinkService
    {
        private readonly NSCRegDbContext _dbContext;
        private readonly Common _commonSvc;

        public LinkService(NSCRegDbContext dbContext)
        {
            _dbContext = dbContext;
            _commonSvc = new Common(dbContext);
        }

        public async Task LinkDelete(LinkCommentM data, string userId)
            => await LinkContext(data, LinkDeleteMethod, nameof(Resource.LinkNotExists), userId);

        public async Task LinkCreate(LinkCommentM data, string userId)
            => await LinkContext(data, LinkCreateMethod, nameof(Resource.LinkTypeInvalid), userId);

        public async Task<List<LinkM>> LinksList(IUnitVm root)
        {
            IStatisticalUnit unit;
            switch (root.Type)
            {
                case StatUnitTypes.EnterpriseGroup:
                    unit = await _commonSvc.GetUnitById<EnterpriseGroup>(root.Id, false);
                    break;
                case StatUnitTypes.EnterpriseUnit:
                    unit = await _commonSvc.GetUnitById<EnterpriseUnit>(root.Id, false,
                        q => q.Include(v => v.EnterpriseGroup));
                    break;
                case StatUnitTypes.LocalUnit:
                    unit = await _commonSvc.GetUnitById<LocalUnit>(root.Id, false,
                        q => q.Include(v => v.EnterpriseUnit).Include(v => v.LegalUnit));
                    break;
                case StatUnitTypes.LegalUnit:
                    unit = await _commonSvc.GetUnitById<LegalUnit>(root.Id, false,
                        q => q.Include(v => v.EnterpriseUnit).Include(v => v.EnterpriseGroup));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var result = new List<LinkM>();

            List<LinkInfo> links;
            var node = Common.ToUnitLookupVm<UnitLookupVm>(unit);

            if (LinksHierarchy.TryGetValue(unit.UnitType, out links))
            {
                links.Select(v => v.Link(unit)).Where(v => v != null).ForEach(v => result.Add(new LinkM
                {
                    Source1 = Common.ToUnitLookupVm<UnitLookupVm>(v),
                    Source2 = node,
                }));
            }

            var nested = await LinksNestedList(root);
            nested.ForEach(v => result.Add(new LinkM
            {
                Source1 = node,
                Source2 = v,
            }));

            return result;
        }
        public async Task<List<UnitLookupVm>> LinksNestedList(IUnitVm unit)
        {
            // TODO: Use LinksHierarchy
            var list = new List<UnitLookupVm>();
            switch (unit.Type)
            {
                case StatUnitTypes.EnterpriseGroup:
                    list.AddRange(Common.ToUnitLookupVm(
                        await _commonSvc.GetUnitsList<EnterpriseUnit>(false)
                            .Where(v => v.EntGroupId == unit.Id).Select(Common.UnitMapping)
                            .Concat(_commonSvc.GetUnitsList<LegalUnit>(false)
                                .Where(v => v.EnterpriseGroupRegId == unit.Id).Select(Common.UnitMapping)
                            ).ToListAsync()
                    ));
                    break;
                case StatUnitTypes.EnterpriseUnit:
                    list.AddRange(Common.ToUnitLookupVm(
                        await _commonSvc.GetUnitsList<LegalUnit>(false)
                            .Where(v => v.EnterpriseUnitRegId == unit.Id).Select(Common.UnitMapping)
                            .Concat(_commonSvc.GetUnitsList<LocalUnit>(false)
                                .Where(v => v.EnterpriseUnitRegId == unit.Id).Select(Common.UnitMapping)
                            ).ToListAsync()
                    ));
                    break;
                case StatUnitTypes.LegalUnit:
                    list.AddRange(Common.ToUnitLookupVm(
                        await _commonSvc.GetUnitsList<LocalUnit>(false)
                            .Where(v => v.LegalUnitId == unit.Id).Select(Common.UnitMapping)
                            .ToListAsync()
                    ));
                    break;
            }
            return list;
        }

        //TODO: Optimize (Use Include instead of second query + another factory)
        public async Task<bool> LinkCanCreate(LinkSubmitM data, string userId)
            => await LinkContext(data, LinkCanCreateMedthod, nameof(Resource.LinkTypeInvalid), userId);

        public async Task<List<UnitNodeVm>> Search(LinkSearchM search)
        {

            var list = new List<IStatisticalUnit>();
            var type = search.Type;

            //TODO: Use LinksHierarchy
            if (type == null || type == StatUnitTypes.EnterpriseGroup)
            {
                list.AddRange(
                    await SearchUnitFilterApply(search, _commonSvc.GetUnitsList<EnterpriseGroup>(false)).ToListAsync());
            }

            if (type == null || type == StatUnitTypes.EnterpriseUnit)
            {
                list.AddRange(
                    await SearchUnitFilterApply(search,
                        _commonSvc.GetUnitsList<EnterpriseUnit>(false).Include(x => x.EnterpriseGroup)).ToListAsync());
            }

            if (type == null || type == StatUnitTypes.LegalUnit)
            {
                list.AddRange(await SearchUnitFilterApply(
                        search,
                        _commonSvc.GetUnitsList<LegalUnit>(false)
                            .Include(x => x.EnterpriseGroup)
                            .Include(x => x.EnterpriseUnit)
                            .ThenInclude(x => x.EnterpriseGroup))
                    .ToListAsync());
            }

            if (type == null || type == StatUnitTypes.LocalUnit)
            {
                list.AddRange(await SearchUnitFilterApply(
                        search,
                        _commonSvc.GetUnitsList<LocalUnit>(false)
                            .Include(x => x.LegalUnit)
                            .ThenInclude(x => x.EnterpriseUnit)
                            .ThenInclude(x => x.EnterpriseGroup)
                            .Include(x => x.LegalUnit)
                            .ThenInclude(x => x.EnterpriseGroup)
                            .Include(x => x.EnterpriseUnit)
                            .ThenInclude(x => x.EnterpriseGroup))
                    .ToListAsync());
            }
            return ToNodeVm(list);
        }

        private static IQueryable<T> SearchUnitFilterApply<T>(LinkSearchM search, IQueryable<T> query)
            where T : IStatisticalUnit
        {
            if (!string.IsNullOrEmpty(search.Wildcard))
            {
                Predicate<string> checkWildcard = superStr => !string.IsNullOrEmpty(superStr) && superStr.ToLower().Contains(search.Wildcard.ToLower());

                query = query.Where(x => checkWildcard(x.Name)
                                                || checkWildcard(x.StatId)
                                                || checkWildcard(x.ExternalId)
                                                || checkWildcard(x.TaxRegId)
                                                || (x.Address != null && checkWildcard(x.Address.AddressPart1)
                                                    || checkWildcard(x.Address.AddressPart2)
                                                    || checkWildcard(x.Address.AddressPart3)));
            }

            if (search.LastChangeFrom.HasValue)
                query = query.Where(x => x.StartPeriod >= search.LastChangeFrom);

            if (search.LastChangeTo.HasValue)
                query = query.Where(x => x.StartPeriod <= search.LastChangeTo);

            if (!string.IsNullOrEmpty(search.DataSource))
                query = query.Where(x => x.DataSource != null && x.DataSource.ToLower().Contains(search.DataSource.ToLower()));

            if (!string.IsNullOrEmpty(search.RegionCode))
                query = query.Where(x => x.Address.Region.Code == search.RegionCode);

            return query;
        }

        private static readonly Dictionary<Tuple<StatUnitTypes, StatUnitTypes>, LinkInfo> LinksMetadata = new[]
        {
            LinkInfo.Create<EnterpriseGroup, EnterpriseUnit>(v => v.EntGroupId, v => v.EnterpriseGroup),
            LinkInfo.Create<EnterpriseGroup, LegalUnit>(v => v.EnterpriseGroupRegId, v => v.EnterpriseGroup),
            LinkInfo.Create<EnterpriseUnit, LegalUnit>(v => v.EnterpriseUnitRegId, v => v.EnterpriseUnit),
            LinkInfo.Create<EnterpriseUnit, LocalUnit>(v => v.EnterpriseUnitRegId, v => v.EnterpriseUnit),
            LinkInfo.Create<LegalUnit, LocalUnit>(v => v.LegalUnitId, v => v.LegalUnit),
        }.ToDictionary(v => Tuple.Create(v.Type1, v.Type2));

        private static readonly Dictionary<StatUnitTypes, List<LinkInfo>> LinksHierarchy =
            LinksMetadata
                .GroupBy(v => v.Key.Item2, v => v.Value)
                .ToDictionary(v => v.Key, v => v.ToList());

        private async Task<bool> LinkDeleteHandler<TParent, TChild>(
            LinkCommentM data,
            bool reverted,
            Func<TChild, int?> idGetter,
            Action<TChild, int?> idSetter,
            string userId)
            where TParent : class, IStatisticalUnit, new()
            where TChild : class, IStatisticalUnit, new()
            => await LinkHandler<TParent, TChild, bool>(
                data,
                reverted,
                async (unit1, unit2) =>
                {
                    var parentId = idGetter(unit2);
                    if (!parentId.HasValue || parentId.Value != unit1.RegId)
                    {
                        throw new BadRequestException(nameof(Resource.LinkNotExists));
                    }
                    idSetter(unit2, null);

                    var changeDateTime = DateTime.Now;
                    _commonSvc.TrackUnithistoryFor<TParent>(unit1.RegId, userId, ChangeReasons.Edit, data.Comment, changeDateTime);
                    _commonSvc.TrackUnithistoryFor<TChild>(unit2.RegId, userId, ChangeReasons.Edit, data.Comment, changeDateTime);

                    await _dbContext.SaveChangesAsync();
                    return true;
                });

        private async Task<bool> LinkCreateHandler<TParent, TChild>(
            LinkCommentM data,
            bool reverted,
            Func<TChild, int?> idGetter,
            Action<TChild, int?> idSetter,
            string userId)
            where TParent : class, IStatisticalUnit, new()
            where TChild : class, IStatisticalUnit, new()
            => await LinkHandler<TParent, TChild, bool>(
                data,
                reverted,
                async (unit1, unit2) =>
                {
                    var parentId = idGetter(unit2);

                    if (parentId.HasValue)
                    {
                        if (parentId == unit1.RegId)
                        {
                            throw new BadRequestException(nameof(Resource.LinkAlreadyExists));
                        }
                        //TODO: Discuss overwrite process throw new BadRequestException(nameof(Resource.LinkUnitAlreadyLinked));
                    }
                    idSetter(unit2, unit1.RegId);

                    var changeDateTime = DateTime.Now;
                    _commonSvc.TrackUnithistoryFor<TParent>(unit1.RegId, userId, ChangeReasons.Edit, data.Comment, changeDateTime);
                    _commonSvc.TrackUnithistoryFor<TChild>(unit2.RegId, userId, ChangeReasons.Edit, data.Comment, changeDateTime);

                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception e)
                        {
                            throw new BadRequestException(nameof(Resource.SaveError), e);
                        }
                        
                    }
                        
                });
        private async Task<bool> LinkCanCreateHandler<TParent, TChild>(
            LinkSubmitM data,
            bool reverted,
            Func<TChild, int?> idGetter,
            Action<TChild, int?> idSetter,
            string userId)
            where TParent : class, IStatisticalUnit
            where TChild : class, IStatisticalUnit, new()
            => await LinkHandler<TParent, TChild, bool>(data, reverted, (unit1, unit2) =>
            {
                var childUnitId = idGetter(unit2);
                return Task.FromResult(childUnitId == null || childUnitId.Value == unit1.RegId);
            });


        private async Task<TResult> LinkHandler<TParent, TChild, TResult>(
            LinkSubmitM data,
            bool reverted,
            Func<TParent, TChild, Task<TResult>> work)
            where TParent : class, IStatisticalUnit
            where TChild : class, IStatisticalUnit
            => await work(
                await _commonSvc.GetUnitById<TParent>(reverted ? data.Source2.Id : data.Source1.Id, false),
                await _commonSvc.GetUnitById<TChild>(reverted ? data.Source1.Id : data.Source2.Id, false));

        private static readonly MethodInfo LinkCreateMethod =
            typeof(LinkService).GetMethod(nameof(LinkCreateHandler),
                BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo LinkDeleteMethod =
            typeof(LinkService).GetMethod(nameof(LinkDeleteHandler),
                BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly MethodInfo LinkCanCreateMedthod =
            typeof(LinkService).GetMethod(nameof(LinkCanCreateHandler),
                BindingFlags.NonPublic | BindingFlags.Instance);

        private async Task<bool> LinkContext<T>(T data, MethodInfo linkMethod, string lookupFailureMessage, string userId)
            where T : LinkSubmitM
        {
            LinkInfo info;
            var reverted = false;
            if (!LinksMetadata.TryGetValue(Tuple.Create(data.Source1.Type, data.Source2.Type), out info))
            {
                if (!LinksMetadata.TryGetValue(Tuple.Create(data.Source2.Type, data.Source1.Type), out info))
                {
                    throw new BadRequestException(lookupFailureMessage);
                }
                reverted = true;
            }

            var method = linkMethod.MakeGenericMethod(
                StatisticalUnitsTypeHelper.GetStatUnitMappingType(info.Type1),
                StatisticalUnitsTypeHelper.GetStatUnitMappingType(info.Type2)
            );
            return await (Task<bool>) method.Invoke(this, new[] {data, reverted, info.Getter, info.Setter, userId});
        }

        private List<UnitNodeVm> ToNodeVm(List<IStatisticalUnit> nodes)
        {
            var result = new List<UnitNodeVm>();
            var visited = new Dictionary<Tuple<int, StatUnitTypes>, UnitNodeVm>();
            var stack = new Stack<Tuple<IStatisticalUnit, UnitNodeVm>>();
            foreach (var root in nodes)
            {
                stack.Push(Tuple.Create(root, (UnitNodeVm) null));
            }
            while (stack.Count != 0)
            {
                var pair = stack.Pop();
                var unit = pair.Item1;
                var child = pair.Item2;

                var key = Tuple.Create(unit.RegId, unit.UnitType);
                UnitNodeVm node;
                if (visited.TryGetValue(key, out node))
                {
                    if (child == null)
                    {
                        node.Highlight = true;
                        continue;
                    }
                    if (node.Children == null)
                    {
                        node.Children = new List<UnitNodeVm> {child};
                    }
                    else
                    {
                        if (node.Children.All(v => v.Id != child.Id && v.Type != child.Type))
                        {
                            node.Children.Add(child);
                        }
                    }
                    continue;
                }
                node = Common.ToUnitLookupVm<UnitNodeVm>(unit);
                if (child != null)
                {
                    node.Children = new List<UnitNodeVm> {child};
                }
                else
                {
                    node.Highlight = true;
                }
                visited.Add(key, node);

                List<LinkInfo> links;
                var isRootNode = true;
                if (LinksHierarchy.TryGetValue(unit.UnitType, out links))
                {
                    foreach (
                        var parentNode in
                        links.Select(v => v.Link(unit))
                            .Where(x => x != null && x.ParentId == null && x.IsDeleted == false))
                    {
                        isRootNode = false;
                        stack.Push(Tuple.Create(parentNode, node));
                    }
                }
                if (isRootNode)
                {
                    result.Add(node);
                }
            }
            return result;
        }
    }
}
