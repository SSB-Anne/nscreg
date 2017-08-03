import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { compose, lifecycle } from 'recompose'
import { equals } from 'ramda'

import { getText } from 'helpers/locale'
import { log } from '../actions'
import QueueLog from './QueueLog'

const { clear, fetchLog } = log
const mapStateToProps = (state, props) => ({
  ...state.datasourcesqueue.log,
  query: props.location.query,
  localize: getText(state.locale),
})

const mapDispatchToProps = (dispatch, props) =>
  bindActionCreators(
    { clear, fetchLog: fetchLog(props.params.id) },
    dispatch,
  )

const hooks = {
  componentDidMount() {
    this.props.fetchLog(this.props.query)
  },

  componentWillReceiveProps(nextProps) {
    if (!equals(nextProps.query, this.props.query)) {
      nextProps.fetchLog(nextProps.query)
    }
  },

  shouldComponentUpdate(nextProps, nextState) {
    return this.props.localize.lang !== nextProps.localize.lang
      || !equals(this.props, nextProps)
      || !equals(this.state, nextState)
  },

  componentWillUnmount() {
    this.props.clear()
  },
}

export default compose(
  connect(mapStateToProps, mapDispatchToProps),
  lifecycle(hooks),
)(QueueLog)
