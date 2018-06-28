import React from 'react'
import { arrayOf, func, number, oneOfType, shape, string, bool } from 'prop-types'
import { Confirm, Header, Loader, Table } from 'semantic-ui-react'
import { equals } from 'ramda'
import { statUnitTypes } from 'helpers/enums'

import Paginate from 'components/Paginate'
import SearchForm from '../SearchForm'
import ListItem from './ListItem'
import styles from './styles.pcss'
import TableHeader from './TableHeader'

class Search extends React.Component {
  static propTypes = {
    fetchData: func.isRequired,
    updateFilter: func.isRequired,
    setQuery: func.isRequired,
    deleteStatUnit: func.isRequired,
    formData: shape({}).isRequired,
    statUnits: arrayOf(shape({
      regId: number.isRequired,
      name: string.isRequired,
    })),
    query: shape({
      wildcard: string,
      includeLiquidated: string,
    }),
    totalCount: oneOfType([number, string]),
    localize: func.isRequired,
    isLoading: bool.isRequired,
    lookups: shape({}).isRequired,
  }

  static defaultProps = {
    query: shape({
      wildcard: '',
      includeLiquidated: false,
    }),
    statUnits: [],
    totalCount: 0,
  }

  state = {
    showConfirm: false,
    selectedUnit: undefined,
    showLegalFormColumn: true,
  }

  handleChangeForm = (name, value) => {
    this.props.updateFilter({ [name]: value })
  }

  handleSubmitForm = (e) => {
    e.preventDefault()
    const { fetchData, setQuery, query, formData } = this.props
    this.setState({
      showLegalFormColumn:
        statUnitTypes.get(formData.type) === undefined ||
        statUnitTypes.get(formData.type) === 'LegalUnit',
    })
    if (equals(query, formData)) fetchData(query)
    else setQuery({ ...query, ...formData })
  }

  handleConfirm = () => {
    const unit = this.state.selectedUnit
    this.setState({ selectedUnit: undefined, showConfirm: false })
    const { query, formData } = this.props
    const queryParams = { ...query, ...formData }
    this.props.deleteStatUnit(unit.type, unit.regId, queryParams)
  }

  handleCancel = () => {
    this.setState({ showConfirm: false })
  }

  displayConfirm = (statUnit) => {
    this.setState({ selectedUnit: statUnit, showConfirm: true })
  }

  renderRow = item => (
    <ListItem
      key={`${item.regId}_${item.type}_${item.name}`}
      statUnit={item}
      deleteStatUnit={this.displayConfirm}
      localize={this.props.localize}
      lookups={this.props.lookups}
      showLegalFormColumn={this.state.showLegalFormColumn}
    />
  )

  renderConfirm() {
    return (
      <Confirm
        open={this.state.showConfirm}
        header={`${this.props.localize('AreYouSure')}?`}
        content={`${this.props.localize('DeleteStatUnitMessage')} "${
          this.state.selectedUnit.name
        }"?`}
        onConfirm={this.handleConfirm}
        onCancel={this.handleCancel}
        confirmButton={this.props.localize('Ok')}
        cancelButton={this.props.localize('ButtonCancel')}
      />
    )
  }

  render() {
    const { statUnits, formData, localize, totalCount, isLoading } = this.props

    return (
      <div className={styles.root}>
        <h2>{localize('SearchStatisticalUnits')}</h2>
        {this.state.showConfirm && this.renderConfirm()}
        <br />
        <SearchForm
          formData={formData}
          onChange={this.handleChangeForm}
          onSubmit={this.handleSubmitForm}
          localize={localize}
          disabled={isLoading}
        />

        <Paginate totalCount={Number(totalCount)}>
          {isLoading && (
            <div className={styles['loader-wrapper']}>
              <Loader active size="massive" />
            </div>
          )}
          {!isLoading &&
            (statUnits.length > 0 ? (
              <Table selectable fixed>
                <TableHeader
                  localize={localize}
                  showLegalFormColumn={this.state.showLegalFormColumn}
                />
                {statUnits.map(this.renderRow)}
              </Table>
            ) : (
              <Header as="h2" content={localize('ListIsEmpty')} textAlign="center" disabled />
            ))}
        </Paginate>
      </div>
    )
  }
}

export default Search
