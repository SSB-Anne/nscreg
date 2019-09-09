import React from 'react'
import { func, arrayOf, shape, string, number, oneOfType, bool } from 'prop-types'
import { Item, Confirm } from 'semantic-ui-react'
import { equals, isEmpty } from 'ramda'

import { getSearchFormErrors, getCorrectQuery } from 'helpers/validation'
import Paginate from 'components/Paginate'
import SearchForm from '../SearchForm'
import ListItem from './ListItem'
import styles from './styles.pcss'

class DeletedList extends React.Component {
  static propTypes = {
    actions: shape({
      updateFilter: func.isRequired,
      setQuery: func.isRequired,
      fetchData: func.isRequired,
      restore: func.isRequired,
      clearSearchFormForDeleted: func.isRequired,
      setSearchConditionForDeleted: func.isRequired,
    }).isRequired,
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
    locale: string.isRequired,
    isLoading: bool.isRequired,
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
    displayConfirm: false,
    selectedUnit: undefined,
  }

  handleChangeForm = (name, value) => {
    this.props.actions.updateFilter({ [name]: value })
  }

  handleSubmitForm = (e) => {
    e.preventDefault()
    const {
      actions: { setQuery },
      query,
      formData,
    } = this.props
    if (!isEmpty(formData)) {
      const qdata = getCorrectQuery({ ...query, ...formData })
      qdata.page = 1
      setQuery(qdata)
    }
  }

  showConfirm = (unit) => {
    this.setState({ selectedUnit: unit, displayConfirm: true })
  }

  handleConfirm = () => {
    const unit = this.state.selectedUnit
    const { query, formData } = this.props
    const queryParams = { ...query, ...formData }
    this.setState({ selectedUnit: undefined, displayConfirm: false })
    this.props.actions.restore(unit.type, unit.regId, queryParams)
  }

  handleCancel = () => {
    this.setState({ selectedUnit: undefined, displayConfirm: false })
  }

  renderConfirm = () => (
    <Confirm
      open={this.state.displayConfirm}
      header={`${this.props.localize('AreYouSure')}?`}
      content={`${this.props.localize('UndeleteStatUnitMessage')} "${
        this.state.selectedUnit.name
      }"?`}
      onConfirm={this.handleConfirm}
      onCancel={this.handleCancel}
    />
  )

  renderRow = item => (
    <ListItem
      key={`${item.regId}_${item.type}`}
      statUnit={item}
      restore={this.showConfirm}
      localize={this.props.localize}
    />
  )

  handleResetForm = () => {
    this.props.actions.clearSearchFormForDeleted()
    this.props.actions.setQuery({})
  }

  render() {
    const {
      formData,
      localize,
      totalCount,
      statUnits,
      isLoading,
      locale,
      actions: { setSearchConditionForDeleted },
    } = this.props
    const searchFormErrors = getSearchFormErrors(formData, localize)

    return (
      <div className={styles.root}>
        {this.state.displayConfirm && this.renderConfirm()}
        <h2>{localize('SearchDeletedStatisticalUnits')}</h2>
        <SearchForm
          formData={formData}
          onChange={this.handleChangeForm}
          onSubmit={this.handleSubmitForm}
          onReset={this.handleResetForm}
          setSearchCondition={setSearchConditionForDeleted}
          locale={locale}
          errors={searchFormErrors}
          localize={localize}
          disabled={isLoading}
        />
        <Paginate totalCount={Number(totalCount)}>
          <Item.Group divided className={styles.items}>
            {statUnits.map(this.renderRow)}
          </Item.Group>
        </Paginate>
      </div>
    )
  }
}

export default DeletedList
