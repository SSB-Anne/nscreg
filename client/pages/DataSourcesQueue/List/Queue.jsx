import React from 'react'
import { func, arrayOf, bool, shape, number, string } from 'prop-types'
import { Segment, Table } from 'semantic-ui-react'

import { getDate, formatDate, getDateSubstrictMonth } from 'helpers/dateHelper'
import Paginate from 'components/Paginate'
import Item from './Item'
import SearchForm from './SearchForm'

const headerKeys = [
  'DataSourceName',
  'DataSourceTemplateName',
  'UploadDateTime',
  'UserName',
  'Status',
]

const Queue = ({
  query,
  result,
  localize,
  fetching,
  totalCount,
  formData,
  actions: { setQuery, updateQueueFilter },
}) => {
  const handleChangeForm = (name, value) => {
    updateQueueFilter({ [name]: value })
  }

  const handleSubmitForm = (e) => {
    e.preventDefault()
    setQuery({ ...query, ...formData })
  }

  return (
    <div>
      <h2>{localize('DataSourceQueues')}</h2>
      <Segment loading={fetching}>
        <SearchForm
          searchQuery={formData}
          onChange={handleChangeForm}
          onSubmit={handleSubmitForm}
          localize={localize}
        />
        <br />
        <br />
        <br />
        <Paginate totalCount={Number(totalCount)}>
          <Table selectable size="small" className="wrap-content" fixed>
            <Table.Header>
              <Table.Row>
                {headerKeys.map(key => <Table.HeaderCell key={key} content={localize(key)} />)}
                <Table.HeaderCell />
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {result.map(item => <Item key={item.id} data={item} localize={localize} />)}
            </Table.Body>
          </Table>
        </Paginate>
      </Segment>
    </div>
  )
}

Queue.propTypes = {
  localize: func.isRequired,
  result: arrayOf(shape({})).isRequired,
  totalCount: number.isRequired,
  actions: shape({
    updateQueueFilter: func.isRequired,
    setQuery: func.isRequired,
  }).isRequired,
  fetching: bool.isRequired,
  formData: shape({}).isRequired,
  query: shape({
    status: string,
    dateTo: string,
    dateFrom: string,
  }),
}

Queue.defaultProps = {
  query: {
    status: 'any',
    dateTo: formatDate(getDate()),
    dateFrom: formatDate(getDateSubstrictMonth()),
  },
}

export default Queue
