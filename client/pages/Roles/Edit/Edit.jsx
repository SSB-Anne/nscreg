import React from 'react'
import { func, shape, oneOfType, string, number, arrayOf } from 'prop-types'
import { Button, Form, Loader, Icon } from 'semantic-ui-react'
import { equals } from 'ramda'

import DataAccess from 'components/DataAccess'
import ActivityTree from 'components/ActivityTree'
import FunctionalAttributes from 'components/FunctionalAttributes'
import styles from './styles.pcss'
import { roles } from 'helpers/enums'

class Edit extends React.Component {
  static propTypes = {
    id: oneOfType([number, string]).isRequired,
    activityTree: arrayOf(shape({})).isRequired,
    role: shape({}).isRequired,
    editForm: func.isRequired,
    fetchRole: func.isRequired,
    fetchActivityTree: func.isRequired,
    submitRole: func.isRequired,
    navigateBack: func.isRequired,
    localize: func.isRequired,
  }

  componentDidMount() {
    this.props.fetchActivityTree()
    this.props.fetchRole(this.props.id)
  }

  shouldComponentUpdate(nextProps, nextState) {
    return (
      this.props.localize.lang !== nextProps.localize.lang ||
      !equals(this.props, nextProps) ||
      !equals(this.state, nextState)
    )
  }

  setRegion = (region) => {
    this.props.editForm({ name: 'region', value: region })
  }

  setActivities = (activities) => {
    this.props.editForm({ name: 'activiyCategoryIds', value: activities.filter(x => x !== 'all') })
  }

  handleEdit = (e, { name, value }) => {
    this.props.editForm({ name, value })
  }

  handleSubmit = (e) => {
    e.preventDefault()
    this.props.submitRole({ ...this.props.role })
  }

  handleAccessToSystemFunctionsChange = e =>
    this.props.editForm({
      name: e.name,
      value: e.checked
        ? [...this.props.role.accessToSystemFunctions, e.value]
        : this.props.role.accessToSystemFunctions.filter(x => x !== e.value),
    })

  render() {
    const { role, activityTree, navigateBack, localize } = this.props
    return (
      <div className={styles.roleEdit}>
        {role === undefined ? (
          <Loader active />
        ) : (
          <Form className={styles.form} onSubmit={this.handleSubmit}>
            <h2>{localize('EditRole')}</h2>
            <Form.Input
              value={role.name}
              onChange={this.handleEdit}
              name="name"
              label={localize('RoleName')}
              placeholder={localize('RoleNamePlaceholder')}
              required
              disabled
            />
            <Form.Input
              value={role.description}
              onChange={this.handleEdit}
              name="description"
              label={localize('Description')}
              placeholder={localize('RoleDescriptionPlaceholder')}
            />
            {role.name !== roles.admin &&
            <DataAccess
              value={role.standardDataAccess}
              name="standardDataAccess"
              label={localize('DataAccess')}
              onChange={this.handleEdit}
              localize={localize}
              readEditable={role.name === roles.nsc || role.name === roles.external}
              writeEditable={role.name === roles.nsc}
            />}
            { /* activityTree &&
            <ActivityTree
              name="activiyCategoryIds"
              label="ActivityCategoryLookup"
              dataTree={activityTree}
              checked={role.activiyCategoryIds}
              callBack={this.setActivities}
              localize={localize}
            /> */
            }
            {/* <FunctionalAttributes
              label={localize('AccessToSystemFunctions')}
              value={role.accessToSystemFunctions}
              onChange={this.handleAccessToSystemFunctionsChange}
              name="accessToSystemFunctions"
              localize={localize}
            /> */}
            <Button
              content={localize('Back')}
              onClick={navigateBack}
              icon={<Icon size="large" name="chevron left" />}
              size="small"
              color="grey"
              type="button"
            />
            <Button content={localize('Submit')} className={styles.sybbtn} type="submit" primary />
          </Form>
        )}
      </div>
    )
  }
}

export default Edit
