import React from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router'
import { Button, Table, Segment, Form, Confirm } from 'semantic-ui-react'

import { checkSystemFunction as sF } from 'helpers/config'
import Paginate from 'components/Paginate'

class List extends React.Component {
  state = { id: undefined }

  askDelete = id => () => this.setState({ id })

  cancelDelete = () => this.setState({ id: undefined })

  confirmDelete = () => {
    const { id } = this.state
    this.setState({ id: undefined }, () => this.props.deleteSampleFrame(id))
  }

  handleEdit = (_, { name, value }) => this.props.updateFilter({ [name]: value })

  handleSubmit = (e) => {
    e.preventDefault()
    this.props.setQuery(this.props.formData)
  }

  renderConfirm() {
    const { result, localize } = this.props
    const { name } = result.find(x => x.id === this.state.id)
    return (
      <Confirm
        onConfirm={this.confirmDelete}
        onCancel={this.cancelDelete}
        header={`${localize('AreYouSure')}?`}
        content={`${localize('DeleteSampleFrameMessage')} "${name}"?`}
        open
      />
    )
  }

  render() {
    const { formData, result, totalCount, localize } = this.props
    const canPreview = sF('SampleFramesView')
    const canEdit = sF('SampleFramesEdit')
    const canDelete = sF('SampleFramesDelete')
    return (
      <div>
        <h2>{localize('SampleFrames')}</h2>
        {canEdit && (
          <Button
            as={Link}
            to="/sampleframes/create"
            content={localize('CreateSampleFrame')}
            icon="add square"
            size="medium"
            color="green"
          />
        )}
        {this.state.id != null && this.renderConfirm()}
        <Segment>
          <Form onSubmit={this.handleSubmit}>
            <Form.Input
              name="wildcard"
              value={formData.wildcard}
              onChange={this.handleEdit}
              label={localize('SampleFramesWildcard')}
            />
            <Button
              type="submit"
              content={localize('Search')}
              icon="search"
              primary
              floated="right"
            />
            <br />
            <br />
            <br />
          </Form>
          <Paginate totalCount={Number(totalCount)}>
            <Table selectable size="small" fixed>
              <Table.Header>
                <Table.Row>
                  <Table.HeaderCell content={localize('Name')} width="5" />
                  <Table.HeaderCell content={localize('Description')} width="5" />
                  {(canPreview || canDelete) && <Table.HeaderCell />}
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {result.map(x => (
                  <Table.Row key={x.id}>
                    <Table.Cell>
                      {canEdit ? <Link to={`/sampleframes/${x.id}`}>{x.name}</Link> : x.name}
                    </Table.Cell>
                    <Table.Cell>{x.description}</Table.Cell>
                    {(canDelete || canPreview) && (
                      <Table.Cell>
                        {canDelete && (
                          <Button
                            onClick={this.askDelete(x.id)}
                            icon="trash"
                            size="mini"
                            color="red"
                            floated="right"
                          />
                        )}
                        {canPreview && (
                          <Button
                            as={Link}
                            to={`/sampleframes/preview/${x.id}`}
                            content={localize('PreviewSampleFrame')}
                            icon="search"
                            color="blue"
                            size="mini"
                            floated="right"
                          />
                        )}
                        {canPreview && (
                          <Button
                            as="a"
                            href={`/api/sampleframes/${x.id}/preview/download`}
                            target="__blank"
                            content={localize('DownloadSampleFrame')}
                            icon="download"
                            color="blue"
                            size="mini"
                            floated="right"
                          />
                        )}
                      </Table.Cell>
                    )}
                  </Table.Row>
                ))}
              </Table.Body>
            </Table>
          </Paginate>
        </Segment>
      </div>
    )
  }
}

const { arrayOf, func, number, shape, string } = PropTypes
List.propTypes = {
  formData: shape({
    wildcard: string.isRequired,
  }).isRequired,
  result: arrayOf(shape({
    id: number.isRequired,
    name: string.isRequired,
  })).isRequired,
  totalCount: number.isRequired,
  setQuery: func.isRequired,
  updateFilter: func.isRequired,
  deleteSampleFrame: func.isRequired,
  localize: func.isRequired,
}

export default List
