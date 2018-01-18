import React from 'react'
import { arrayOf, func, shape, string } from 'prop-types'
import { Grid, Label, Header, Segment } from 'semantic-ui-react'
import R from 'ramda'

import ListWithDnd from 'components/ListWithDnd'
import colors from 'helpers/colors'
import Item from './Item'
import styles from './styles.pcss'

const resetSelection = ({ hovered }) => ({
  left: undefined,
  right: undefined,
  dragStarted: false,
  hovered,
})

class MappingsEditor extends React.Component {
  static propTypes = {
    attributes: arrayOf(string).isRequired,
    columns: arrayOf(shape({
      name: string.isRequired,
      localizeKey: string.isRequired,
    }).isRequired).isRequired,
    value: arrayOf(arrayOf(string.isRequired).isRequired),
    mandatoryColumns: arrayOf(string),
    onChange: func.isRequired,
    localize: func.isRequired,
  }

  static defaultProps = {
    value: [],
    mandatoryColumns: [],
  }

  state = {
    left: undefined,
    right: undefined,
    dragStarted: false,
    hovered: undefined,
  }

  componentWillReceiveProps(nextProps) {
    const newAttrib =
      this.state.left !== undefined &&
      nextProps.attributes.find(attr => attr === this.state.left) === undefined
    const newColumn =
      this.state.right !== undefined &&
      nextProps.columns.find(col => col.name === this.state.right) === undefined
    if (newAttrib || newColumn) this.setState(resetSelection)
  }

  getOther(prop) {
    return prop === 'left' ? this.state.right : this.state.left
  }

  getAttributeColor(prop, value) {
    const leftIndex = this.props.attributes.indexOf(prop === 'left' ? value : this.props.value.find(([, col]) => col === value)[0])
    return colors[(leftIndex + 1) % colors.length]
  }

  mouseUpIsBeingTracked(prop) {
    return this.state.dragStarted && this.getOther(prop)
  }

  handleAdd(prop, value) {
    const pair = prop === 'left' ? [value, this.state.right] : [this.state.left, value]
    const duplicate = this.props.value.find(m => m[0] === pair[0] && m[1] === pair[1])
    if (duplicate === undefined) {
      const nextValue = this.props.value.filter(m => m[1] !== pair[1]).concat([pair])
      this.setState(resetSelection, () => {
        this.props.onChange(nextValue)
      })
    } else {
      this.setState(resetSelection)
    }
  }

  handleMouseDown = (prop, value) => (e) => {
    e.preventDefault()
    this.setState({
      right: undefined,
      left: undefined,
      [prop]: value,
      dragStarted: true,
    })
  }

  handleMouseUp = (prop, value) => (e) => {
    e.preventDefault()
    document.removeEventListener('mouseup', this.handleMouseUpOutside, false)
    if (this.mouseUpIsBeingTracked(prop)) this.handleAdd(prop, value)
    else this.setState(resetSelection)
  }

  handleMouseUpOutside = () => {
    this.setState(resetSelection)
  }

  handleMouseEnter = (prop, value) => () => {
    this.setState({ hovered: { [prop]: value } }, () => {
      if (this.mouseUpIsBeingTracked(prop)) {
        document.removeEventListener('mouseup', this.handleMouseUpOutside, false)
      }
    })
  }

  handleMouseLeave = () => {
    if (this.state.dragStarted) {
      document.addEventListener('mouseup', this.handleMouseUpOutside, false)
    }
    this.setState({ hovered: undefined })
  }

  handleClick = (prop, value) => () => {
    if (!this.state[prop]) this.setState({ [prop]: value })
    else if (this.getOther(prop)) this.handleAdd(prop, value)
  }

  renderItem(prop, value, label) {
    const adopt = f => f(prop, value)
    const index = this.props.value.findIndex(x => x[prop === 'left' ? 0 : 1] === value)
    const { hovered } = this.state
    return (
      <Item
        key={value}
        id={`${prop}_${value}`}
        text={label || value}
        selected={this.state[prop] === value}
        onClick={adopt(this.handleClick)}
        onMouseDown={adopt(this.handleMouseDown)}
        onMouseUp={adopt(this.handleMouseUp)}
        onMouseEnter={adopt(this.handleMouseEnter)}
        onMouseLeave={this.handleMouseLeave}
        hovered={hovered !== undefined && hovered[prop] === value}
        pointing={index >= 0 ? (prop === 'left' ? 'right' : 'left') : prop}
        color={prop === 'left' || index >= 0 ? this.getAttributeColor(prop, value) : 'grey'}
      />
    )
  }

  render() {
    const {
      attributes,
      columns,
      value: mappings,
      mandatoryColumns: mandatoryCols,
      onChange,
      localize,
    } = this.props
    const labelColumn = key =>
      key.includes('.')
        ? key
          .split('.')
          .map((x, i) => (i === 0 && mandatoryCols.includes(x) ? `${localize(x)}*` : localize(x)))
          .join(' > ')
        : mandatoryCols.includes(key) ? `${localize(key)}*` : localize(key)
    const renderValueItem = ([attr, col]) => {
      const color = this.getAttributeColor('left', attr)
      const colText = labelColumn(columns.find(c => c.name === col).localizeKey)
      const onRemove = () => onChange(R.without([[attr, col]], mappings))
      return (
        <Label.Group>
          <Label content={attr} pointing="right" color={color} basic />
          <Label content={colText} onRemove={onRemove} pointing="left" color={color} basic />
        </Label.Group>
      )
    }
    return (
      <Grid columns={3} stackable>
        <Grid.Row>
          <Grid.Column width={4}>
            <Header content={localize('VariablesOfDataSource')} as="h5" />
            <Segment>{attributes.map(x => this.renderItem('left', x))}</Segment>
          </Grid.Column>
          <Grid.Column width={4}>
            <Header content={localize('VariablesOfDatabase')} as="h5" />
            <Segment>
              {columns.map(x => this.renderItem('right', x.name, labelColumn(x.localizeKey)))}
            </Segment>
          </Grid.Column>
          <Grid.Column width={8} textAlign="center">
            <Header content={localize('VariablesMappingResults')} as="h5" />
            <Segment>
              <ListWithDnd
                value={mappings}
                onChange={onChange}
                renderItem={renderValueItem}
                getItemKey={R.join('-')}
                listProps={{ className: styles['values-root'] }}
                listItemProps={{ className: styles['mappings-item'] }}
              />
            </Segment>
          </Grid.Column>
        </Grid.Row>
      </Grid>
    )
  }
}

export default MappingsEditor