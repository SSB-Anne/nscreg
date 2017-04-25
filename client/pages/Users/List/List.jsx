import React from 'react'
import { Link } from 'react-router'
import { Button, Icon, Segment } from 'semantic-ui-react'
import Griddle, { RowDefinition, ColumnDefinition } from 'griddle-react'
import R from 'ramda'

import { systemFunction as sF } from 'helpers/checkPermissions'
import { wrapper } from 'helpers/locale'
import statuses from 'helpers/userStatuses'
import {
  griddleSemanticStyle,
  EnhanceWithRowData,
  GriddleDateColumn,
  GriddlePaginationMenu,
  GriddlePagination,
  GriddleSortableColumn,
  GriddleNoResults,
} from 'components/GriddleExt'

import FilterList from './FilterList'
import ColumnActions from './ColumnActions'
import styles from './styles'

const ColumnUserName = EnhanceWithRowData(({ rowData }) => (
  <span>
    {sF('UserEdit')
      ? <Link to={`/users/edit/${rowData.id}`}>{rowData.name}</Link>
      : rowData.name
    }
  </span>
  ),
)

const ColumnRoles = EnhanceWithRowData(({ rowData }) => (
  <span>
    {rowData.roles.map(v => v.name).join(', ')}
  </span>
  ),
)

const ColumnStatus = localize => ({ value }) => (
  <span> {localize(statuses.filter(v => v.key === value)[0].value)}</span>
)

const UserActions = (localize, setUserStatus, getFilter) =>
  EnhanceWithRowData(({ rowData }) => (
    <ColumnActions
      rowData={rowData}
      localize={localize}
      setUserStatus={setUserStatus}
      getFilter={getFilter}
    />
  ))

const { func, bool, shape, number } = React.PropTypes

class UsersList extends React.Component {

  static propTypes = {
    localize: func.isRequired,
    fetchUsers: func.isRequired,
    isLoading: bool.isRequired,
    filter: shape({
      page: number.isRequired,
      pageSize: number.isRequired,
    }).isRequired,
  }

  componentDidMount() {
    const { filter } = this.props
    this.props.fetchUsers(filter)
  }

  shouldComponentUpdate(nextProps, nextState) {
    return this.props.localize.lang !== nextProps.localize.lang
      || !R.equals(this.props, nextProps)
      || !R.equals(this.state, nextState)
  }

  onNext = () => {
    const { filter } = this.props
    this.props.fetchUsers({ ...filter, page: filter.page + 1 })
  }

  onPrevious = () => {
    const { filter } = this.props
    this.props.fetchUsers({ ...filter, page: filter.page - 1 })
  }

  onGetPage = (pageNumber) => {
    const { filter } = this.props
    this.props.fetchUsers({ ...filter, page: pageNumber })
  }

  onFilter = (data) => {
    const { filter } = this.props
    this.props.fetchUsers({ ...filter, ...data, page: 1 })
  }

  onSort = (sort) => {
    const { filter } = this.props
    switch (sort.id) {
      case 'name':
      case 'regionName':
      case 'creationDate':
        this.props.fetchUsers({
          ...filter,
          sortColumn: sort.id,
          sortAscending: filter.sortColumn !== sort.id || !filter.sortAscending,
        })
        break
      default:
        break
    }
  }

  render() {
    const {
      filter, users, totalCount, totalPages, editUser, setUserStatus, isLoading, localize,
    } = this.props
    return (
      <div>
        <div className={styles['add-user']}>
          <h2>{localize('UsersList')}</h2>
          {sF('UserCreate')
            && <Button
              as={Link} to="/users/create"
              content={localize('CreateUserButton')}
              icon={<Icon size="large" name="user plus" />}
              size="medium"
              color="green"
            />}
        </div>
        <br />
        <div className={styles['list-root']}>
          <div className={styles.addUser} />
          <FilterList onChange={this.onFilter} filter={filter} />
          <Segment vertical loading={isLoading}>
            <Griddle
              data={users}
              pageProperties={{
                currentPage: filter.page,
                pageSize: filter.pageSize,
                recordCount: totalCount,
              }}
              events={{
                onNext: this.onNext,
                onPrevious: this.onPrevious,
                onGetPage: this.onGetPage,
                onSort: this.onSort,
              }}
              components={{
                Filter: () => <span />,
                SettingsToggle: () => <span />,
                Pagination: GriddlePagination,
                PageDropdown: GriddlePaginationMenu,
                NoResults: GriddleNoResults(localize),
              }}
              sortProperties={[{ id: filter.sortColumn, sortAscending: filter.sortAscending }]}
              styleConfig={griddleSemanticStyle}
            >
              <RowDefinition>
                <ColumnDefinition id="name" title={localize('UserName')} customComponent={ColumnUserName} customHeadingComponent={GriddleSortableColumn} width={250} />
                <ColumnDefinition id="description" title={localize('Description')} />
                <ColumnDefinition id="regionName" title={localize('Region')} width={200} customHeadingComponent={GriddleSortableColumn} />
                <ColumnDefinition id="roles" title={localize('Roles')} customComponent={ColumnRoles} width={200} />
                <ColumnDefinition id="creationDate" title={localize('RegistrationDate')} customComponent={GriddleDateColumn} customHeadingComponent={GriddleSortableColumn} width={150} />
                <ColumnDefinition id="status" title={localize('Status')} customComponent={ColumnStatus(localize)} />
                <ColumnDefinition title="&nbsp;" customComponent={UserActions(localize, setUserStatus, () => this.props.filter)} />
              </RowDefinition>
            </Griddle>
          </Segment>
        </div>
      </div>
    )
  }
}

export default wrapper(UsersList)
