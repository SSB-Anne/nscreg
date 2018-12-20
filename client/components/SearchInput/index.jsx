import React from 'react'
import { func, shape, string, bool } from 'prop-types'
import { Form, Search } from 'semantic-ui-react'
import debounce from 'lodash/debounce'
import { equals, isEmpty, isNil } from 'ramda'

import { internalRequest } from 'helpers/request'
import simpleName from './nameCreator'

const waitTime = 250

class SearchInput extends React.Component {
  static propTypes = {
    localize: func.isRequired,
    searchData: shape({
      url: string.isRequired,
      editUrl: string,
      label: string.isRequired,
      placeholder: string.isRequired,
      data: shape({}).isRequred,
    }).isRequired,
    onValueSelected: func.isRequired,
    onValueChanged: func.isRequired,
    required: bool.isRequired,
    disabled: bool.isRequired,
  }

  state = {
    data: this.props.searchData.data,
    results: [],
    isLoading: false,
  }

  componentWillReceiveProps(newProps) {
    const newData = newProps.searchData.data
    if (!isEmpty(newData) && !equals(this.state.data, newData)) {
      this.setState({ data: newData })
    }
  }

  handleSearchResultSelect = (e, { result: { data } }) => {
    e.preventDefault()
    this.setState(
      {
        data: { ...data, name: simpleName(data) },
      },
      () => this.props.onValueSelected(data),
    )
  }

  handleSearchChange = (e, { value }) => {
    if (isNil(value) || isEmpty(value)) {
      return
    }
    this.setState(
      s => ({
        data: { ...s.data, name: value },
        isLoading: true,
      }),
      () => {
        this.props.onValueChanged(value)
        this.search(value)
      },
    )
  }

  search = debounce((params) => {
    internalRequest({
      url: this.props.searchData.url,
      queryParams: { wildcard: params },
      method: 'get',
      onSuccess: (result) => {
        this.setState({
          isLoading: false,
          results: [
            ...result.map(x => ({
              title: simpleName(x),
              description: x.code,
              data: x,
              key: x.code,
            })),
          ],
        })
      },
      onFail: () => {
        this.setState(
          {
            isLoading: false,
            results: [],
          },
          () => {
            this.props.onValueSelected({})
          },
        )
      },
    })
  }, waitTime)

  render() {
    const { localize, searchData, disabled, required } = this.props
    const { isLoading, results } = this.state

    return (
      <Form.Input
        control={Search}
        onResultSelect={this.handleSearchResultSelect}
        onSearchChange={this.handleSearchChange}
        results={results}
        showNoResults={false}
        placeholder={localize(searchData.placeholder)}
        loading={isLoading}
        label={localize(searchData.label)}
        value={searchData.value && searchData.value.name}
        disabled={disabled}
        fluid
        required={required}
      />
    )
  }
}

export default SearchInput
