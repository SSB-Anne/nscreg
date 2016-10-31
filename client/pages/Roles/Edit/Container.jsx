import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import actions from './actions'
import Edit from './Edit'

// TODO: get selected role id
export default connect(
  ({ editRole }) => ({ ...editRole }),
  dispatch => bindActionCreators(actions, dispatch)
)(Edit)
