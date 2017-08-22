import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import StatUnitForm from 'components/StatUnitForm'
import { getText } from 'helpers/locale'
import { actionCreators } from './actions'

const { editForm, navigateBack } = actionCreators

export default connect(
  ({ createStatUnit: { statUnit, type, errors, schema }, locale }, ownProps) => ({
    statUnit,
    errors,
    schema,
    localize: getText(locale),
    ...ownProps,
  }),
  dispatch => bindActionCreators({ onChange: editForm, onCancel: navigateBack }, dispatch),
)(StatUnitForm)
