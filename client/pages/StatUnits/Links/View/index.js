import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { getText } from 'helpers/locale'
import actions from './actions'
import ViewLinks from './ViewLinks'

export default connect(
  ({ viewLinks, locale }) => ({ ...viewLinks, localize: getText(locale) }),
  dispatch => bindActionCreators(actions, dispatch),
)(ViewLinks)
