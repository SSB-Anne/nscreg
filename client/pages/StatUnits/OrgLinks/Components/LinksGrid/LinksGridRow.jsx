import React from 'react'
import { Table, Icon, Popup } from 'semantic-ui-react'

import { systemFunction as sF } from 'helpers/checkPermissions'
import statUnitTypes from 'helpers/statUnitTypes'

const { func, shape, string, number, bool } = React.PropTypes

const shapeOfSource = shape({
  code: string,
  name: string,
  type: number,
})

class LinksGridRow extends React.Component {
  static propTypes = {
    index: number.isRequired,
    localize: func.isRequired,
    deleteLink: func.isRequired,
    data: shape({
      source1: shapeOfSource.isRequired,
      source2: shapeOfSource.isRequired,
    }).isRequired,
    readOnly: bool.isRequired,
  }

  onDeleteClick = () => {
    const { data, deleteLink } = this.props
    deleteLink(data)
  }
  render() {
    const { index, data: { source1, source2 }, localize } = this.props
    return (
      <Table.Row>
        <Table.Cell>{index}</Table.Cell>
        <Table.Cell>{source1.name}</Table.Cell>
        <Table.Cell>{localize(statUnitTypes.get(source1.type))}</Table.Cell>
        <Table.Cell>{source1.code}</Table.Cell>
        <Table.Cell>{source2.name}</Table.Cell>
        <Table.Cell>{localize(statUnitTypes.get(source2.type))}</Table.Cell>
        <Table.Cell>{source2.code}</Table.Cell>
      </Table.Row>
    )
  }
}

export default LinksGridRow
