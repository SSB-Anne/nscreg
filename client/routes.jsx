import React from 'react'
import { IndexRoute, Route } from 'react-router'

import { systemFunction as sF } from 'helpers/checkPermissions'
import Layout from 'layout'
import Home from 'pages/Home'
import About from 'pages/About'
import NotFound from 'pages/NotFound'

import AccountView from 'pages/Account/View'
import AccountEdit from 'pages/Account/Edit'

import RolesRoutes from 'pages/Roles/Routes'
import UsersRoutes from 'pages/Users/Routes'
import StatUnits from 'pages/StatUnits/Routes'
import AddressRoutes from 'pages/Address/Routes'
import RegionsRoutes from 'pages/Regions/Routes'
import DataSourcesRoutes from 'pages/DataSources/Routes'
import DataSourcesQueueRoutes from 'pages/DataSourcesQueue/Routes'
import LogicalChecksRoutes from 'pages/LogicalChecks/Routes'

export default (
  <Route path="/" component={Layout}>
    <IndexRoute component={Home} />
    <Route path="account" component={AccountView} />
    <Route path="account/edit" component={AccountEdit} />
    {sF('RoleView') && RolesRoutes}
    {sF('UserView') && UsersRoutes}
    {sF('StatUnitView') && StatUnits}
    {sF('AddressView') && AddressRoutes}
    {sF('RegionsView') && RegionsRoutes}
    {sF('DataSourcesView') && DataSourcesRoutes}
    {sF('DataSourcesQueueView') && DataSourcesQueueRoutes}
    {sF('StatUnitView') && LogicalChecksRoutes}
    <Route path="about" component={About} />
    <Route path="*" component={NotFound} />
  </Route>
)