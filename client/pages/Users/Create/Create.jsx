import React from 'react'
import { Button, Form, Loader, Message } from 'semantic-ui-react'
import rqst from '../../../helpers/fetch'
import statuses from '../../../helpers/userStatuses'

export default class Create extends React.Component {
  state = {
    rolesList: [],
    fetchingRoles: true,
    rolesFailMessage: undefined,
    password: '',
    confirmPassword: '',
  }
  componentDidMount() {
    this.fetchRoles()
  }
  fetchRoles = () => {
    rqst({
      url: '/api/roles',
      onSuccess: ({ result }) => { this.setState(s => ({
        ...s,
        rolesList: result,
        fetchingRoles: false,
      })) },
      onFail: () => { this.setState(s => ({
        ...s,
        rolesFailMessage: 'failed loading roles',
        fetchingRoles: false,
      })) },
      onError: () => { this.setState(s => ({
        ...s,
        rolesFailMessage: 'error while fetching roles',
        fetchingRoles: false,
      })) },
    })
  }
  renderForm() {
    const { submitUser, message, status } = this.props
    const handleSubmit = (e, serialized) => {
      e.preventDefault()
      submitUser(serialized)
    }
    const handleChange = propName => (e) => {
      e.persist()
      this.setState(s => ({ ...s, [propName]: e.target.value }))
    }
    return (
      <Form onSubmit={handleSubmit}>
        <Form.Input
          name="name"
          label="User name"
          placeholder="e.g. Robert Diggs"
        />
        <Form.Input
          name="login"
          label="User login"
          placeholder="e.g. rdiggs"
        />
        <Form.Input
          value={this.state.password}
          onChange={handleChange('password')}
          name="password"
          type="password"
          label="User password"
          placeholder="type strong password here"
        />
        <Form.Input
          value={this.state.confirmPassword}
          onChange={handleChange('confirmPassword')}
          name="confirmPassword"
          type="password"
          label="Confirm password"
          placeholder="type password again"
          error={this.state.confirmPassword !== this.state.password}
        />
        <Form.Input
          name="email"
          type="email"
          label="User email"
          placeholder="e.g. robertdiggs@site.domain"
        />
        <Form.Input
          name="phone"
          type="tel"
          label="User phone"
          placeholder="555123456"
        />
        {this.state.fetchingRoles
          ? <Loader content="fetching roles" active />
          : <Form.Select
            options={this.state.rolesList.map(r => ({ value: r.name, text: r.name }))}
            name="assignedRoles"
            label="Assigned roles"
            placeholder="select or search roles..."
            multiple
            search
          />}
        <Form.Select
          options={statuses.map(s => ({ value: s.key, text: s.value }))}
          name="status"
          defaultValue={1}
          label="User status"
        />
        <Form.Input
          name="description"
          label="Description"
          placeholder="e.g. very famous NSO employee"
        />
        <Button type="submit" primary>Submit</Button>
        {this.state.rolesFailMessage
          && <div>
            <Message content={this.state.rolesFailMessage} negative />
            <Button onClick={() => { this.fetchRoles() }} type="button">
              try reload roles
            </Button>
          </div>}
        {message && <Message content={message} />}
      </Form>
    )
  }
  render() {
    return (
      <div>
        <h2>Create new user</h2>
        {this.renderForm()}
      </div>
    )
  }
}
