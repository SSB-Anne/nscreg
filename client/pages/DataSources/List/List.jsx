import React from 'react'
import { arrayOf, shape, func, string, number, oneOfType } from 'prop-types'
import { Link } from 'react-router'
import { equals } from 'ramda'
import { Button, Table, Segment, Divider } from 'semantic-ui-react'

import { systemFunction as sF } from 'helpers/checkPermissions'
import Paginate from 'components/Paginate'
import SearchForm from './SearchForm'
import ListItem from './ListItem'

class List extends React.Component {

  static propTypes = {
    formData: shape({
      wildcard: string,
      statUnitType: oneOfType([number, string]),
      priority: oneOfType([number, string]),
      allowedOperations: oneOfType([number, string]),
    }).isRequired,
    query: shape({}).isRequired,
    dataSources: arrayOf(shape({
      id: number.isRequired,
      name: string.isRequired,
    })),
    totalCount: oneOfType([string, number]).isRequired,
    onSubmit: func.isRequired,
    onChange: func.isRequired,
    onItemDelete: func.isRequired,
    localize: func.isRequired,
    fetchData: func.isRequired,
    clear: func.isRequired,
  }

  static defaultProps = {
    dataSources: [],
  }

  state = {
    dataSource: undefined,
    description: '',
  }

  componentDidMount() {
    this.props.fetchData(this.props.query)
  }

  componentWillReceiveProps(nextProps) {
    if (!equals(nextProps.query, this.props.query)) {
      nextProps.fetchData(nextProps.query)
    }
  }

  componentWillUnmount() {
    this.props.clear()
  }

  handleItemDelete = id => () => {
    const { onItemDelete, localize } = this.props
    // eslint-disable-next-line no-alert
    if (window.confirm(localize('AreYouSure'))) onItemDelete(id)
  }

  render() {
    const {
      formData, dataSources, totalCount, onSubmit, onChange, localize,
    } = this.props
    const canEdit = sF('DataSourcesEdit')
    const canDelete = sF('DataSourcesDelete')
    return (
      <div>
        <h2>{localize('DataSources')}</h2>
        <Segment>
          <Button
            as={Link}
            to="/datasources/create"
            content={localize('CreateDataSource')}
            icon="add square"
            size="medium"
            color="green"
          />
          <Divider />
          <SearchForm
            formData={formData}
            onSubmit={onSubmit}
            onChange={onChange}
            localize={localize}
          />
          <Paginate totalCount={Number(totalCount)}>
            <Table selectable size="small" className="wrap-content" fixed>
              <Table.Header>
                <Table.Row>
                  <Table.HeaderCell content={localize('Id')} />
                  <Table.HeaderCell content={localize('Name')} />
                  <Table.HeaderCell content={localize('Description')} />
                  <Table.HeaderCell content={localize('Priority')} />
                  <Table.HeaderCell content={localize('AllowedOperations')} />
                  {canDelete && <Table.HeaderCell />}
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {dataSources.map(ds => (
                  <ListItem
                    key={ds.id}
                    canEdit={canEdit}
                    canDelete={canDelete}
                    onDelete={canDelete
                      ? this.handleItemDelete(ds.id)
                      : undefined}
                    {...ds}
                  />
                ))}
              </Table.Body>
            </Table>
          </Paginate>
        </Segment>
      </div>
    )
  }
}

export default List
