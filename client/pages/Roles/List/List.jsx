import React from 'react'
import { Link } from 'react-router'
import { Button, Icon, List, Loader } from 'semantic-ui-react'

import { systemFunction as sF } from 'helpers/checkPermissions'
import styles from './styles'
import UsersList from './UsersList'

const Item = ({ id, name, description, deleteRole, fetchRoleUsers }) => {
  const handleDelete = () => {
    if (confirm(`Delete role '${name}'. Are you sure?`)) deleteRole(id)
  }
  const handleFetchUsers = () => {
    fetchRoleUsers(id)
  }
  return (
    <List.Item>
      <List.Icon name="suitcase" size="large" verticalAlign="middle" />
      <List.Content>
        <List.Header
          content={sF('RoleEdit')
            ? <Link to={`/roles/edit/${id}`}>{name}</Link>
            : <span>{name}</span>}
        />
        <List.Description>
          <span>{description}</span>
          <Button onClick={handleFetchUsers} animated="vertical" primary>
            <Button.Content hidden>Users</Button.Content>
            <Button.Content visible>
              <Icon name="users" />
            </Button.Content>
          </Button>
          {sF('RoleDelete') && <Button onClick={handleDelete} negative>delete</Button>}
        </List.Description>
      </List.Content>
    </List.Item>
  )
}

export default class RolesList extends React.Component {
  componentDidMount() {
    this.props.fetchRoles()
  }
  renderRoleUsers = role => (
    <div>
      <h3>Users in {role.name} role</h3>
      <UsersList users={role.users} />
    </div>
  )
  render() {
    const {
      roles, totalCount, totalPages, selectedRole, deleteRole, fetchRoleUsers,
    } = this.props
    const role = roles.find(r => r.id === selectedRole)
    return (
      <div>
        <h2>Roles list</h2>
        <div className={styles['list-root']}>
          {sF('RoleCreate') && <Link to="/roles/create">Create</Link>}
          <Loader active={status === 1} />
          <List>
            {roles && roles.map(r =>
              <Item key={r.id} {...{ ...r, deleteRole, fetchRoleUsers }} />)}
          </List>
          <span>total: {totalCount}</span>
          <span>total pages: {totalPages}</span>
        </div>
        {role && role.users && this.renderRoleUsers(role)}
      </div>
    )
  }
}
