import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { lifecycle, compose } from 'recompose'
import { equals } from 'ramda'

import { getText } from 'helpers/locale'
import { list } from '../actions'
import Queue from './Queue'

const mapStateToProps = (state, props) =>
  ({
    ...state.datasourcesqueue.list,
    query: props.location.query,
    localize: getText(state.locale),
  })

const { setQuery, ...actions } = list
const mapDispatchToProps = (dispatch, props) =>
  ({
    actions: {
      ...bindActionCreators(actions, dispatch),
      setQuery: bindActionCreators(setQuery(props.location.pathname), dispatch),
    },
  })

const hooks = {
  componentDidMount() {
    this.props.actions.fetchQueue(this.props.query)
  },

  componentWillReceiveProps(nextProps) {
    if (!equals(nextProps.query, this.props.query)) {
      nextProps.actions.fetchQueue(nextProps.query)
    }
  },

  shouldComponentUpdate(nextProps, nextState) {
    return this.props.localize.lang !== nextProps.localize.lang
      || !equals(this.props, nextProps)
      || !equals(this.state, nextState)
  },

  componentWillUnmount() {
    this.props.actions.clear()
  },
}

export default compose(
  connect(mapStateToProps, mapDispatchToProps),
  lifecycle(hooks),
)(Queue)
