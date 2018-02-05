import React from 'react'
import debounce from 'lodash/debounce'
import R from 'ramda'

export default (Component, delay = 200) =>
  class DebounceFieldWrapper extends React.Component {
    static propTypes = Component.propTypes

    static defaultProps = Component.defaultProps

    static displayName = `Debounced(${Component.displayName || Component.name || 'Field'})`

    state = {
      pending: false,
      e: undefined,
      data: this.props,
    }

    componentWillReceiveProps(nextProps) {
      const nextValueEquals = R.equals(nextProps.value)
      if (!nextValueEquals(this.props.value) && !nextValueEquals(this.state.data.value)) {
        this.setState({ data: nextProps, pending: false }, this.delayedChange.cancel)
      }
    }

    componentWillUnmount() {
      if (this.state.pending) this.delayedChange.flush()
      clearTimeout(this.onBlurTimeout)
    }

    // eslint-disable-next-line react/sort-comp
    immediateChange() {
      this.props.onChange(this.state.e, this.state.data)
    }

    tryImmediateChange() {
      if (this.state.pending) {
        this.setState({ pending: false }, this.immediateChange)
      }
    }

    delayedChange = debounce(this.tryImmediateChange, delay)

    onChange = (e, data) => {
      if (e !== undefined) e.persist()
      this.setState({ e, data, pending: true }, this.delayedChange)
    }

    onBlur = (e) => {
      if (this.state.pending) this.delayedChange.flush()
      if (e !== undefined) e.persist()
      this.onBlurTimeout = setTimeout(() => this.props.onBlur(e), delay)
    }

    onKeyDown = (e) => {
      if (e !== undefined) e.persist()
      if (this.state.pending) {
        if (e.keyCode === 13) this.delayedChange.flush()
      } else if (this.props.onKeyDown) {
        this.props.onKeyDown(e)
      }
    }

    render() {
      const props = {
        ...this.props,
        ...this.state.data,
      }
      if (this.props.onChange !== undefined) props.onChange = this.onChange
      if (this.props.onBlur !== undefined) props.onBlur = this.onBlur
      if (this.props.onKeyDown !== undefined) props.onKeyDown = this.onKeyDown
      return <Component {...props} />
    }
  }
