import React from 'react'
import { Table, Checkbox } from 'semantic-ui-react'
import { wrapper } from 'helpers/locale'
import systemFunctions from 'helpers/systemFunctions'

const FunctionalAttributes = ({ localize, value, onChange, label, name }) => {
  const onChangeCreator = propName => (e, { checked }) => {
    onChange({ name, value: systemFunctions.get(propName), checked })
  }

  const isChecked = fname => value.some(x => x === systemFunctions.get(fname))

  return (
    <div className="field">
      <label>{label}</label>
      <Table definition>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell />
            <Table.HeaderCell>{localize('Read')}</Table.HeaderCell>
            <Table.HeaderCell>{localize('Create')}</Table.HeaderCell>
            <Table.HeaderCell>{localize('Update')}</Table.HeaderCell>
            <Table.HeaderCell>{localize('Delete')}</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          <Table.Row>
            <Table.Cell>{localize('Account')}</Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('AccountView')} checked={isChecked('AccountView')} />
            </Table.Cell>
            <Table.Cell />
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('AccountEdit')} checked={isChecked('AccountEdit')} />
            </Table.Cell>
            <Table.Cell />
          </Table.Row>
          <Table.Row>
            <Table.Cell>{localize('Roles')}</Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('RoleView')} checked={isChecked('RoleView')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('RoleCreate')} checked={isChecked('RoleCreate')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('RoleEdit')} checked={isChecked('RoleEdit')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('RoleDelete')} checked={isChecked('RoleDelete')} />
            </Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>{localize('Users')}</Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('UserView')} checked={isChecked('UserView')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('UserCreate')} checked={isChecked('UserCreate')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('UserEdit')} checked={isChecked('UserEdit')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('UserDelete')} checked={isChecked('UserDelete')} />
            </Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>{localize('StatUnits')}</Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('StatUnitView')} checked={isChecked('StatUnitView')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('StatUnitCreate')} checked={isChecked('StatUnitCreate')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('StatUnitEdit')} checked={isChecked('StatUnitEdit')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('StatUnitDelete')} checked={isChecked('StatUnitDelete')} />
            </Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>{localize('Regions')}</Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('RegionsView')} checked={isChecked('RegionsView')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('RegionsCreate')} checked={isChecked('RegionsCreate')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('RegionsEdit')} checked={isChecked('RegionsEdit')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('RegionsDelete')} checked={isChecked('RegionsDelete')} />
            </Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>{localize('Address')}</Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('AddressView')} checked={isChecked('AddressView')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('AddressCreate')} checked={isChecked('AddressCreate')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('AddressEdit')} checked={isChecked('AddressEdit')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('AddressDelete')} checked={isChecked('AddressDelete')} />
            </Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>{localize('LinkUnits')}</Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('LinksView')} checked={isChecked('LinksView')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('LinksCreate')} checked={isChecked('LinksCreate')} />
            </Table.Cell>
            <Table.Cell />
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('LinksDelete')} checked={isChecked('LinksDelete')} />
            </Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>{localize('Soate')}</Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('SoateView')} checked={isChecked('SoateView')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('SoateCreate')} checked={isChecked('SoateCreate')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('SoateEdit')} checked={isChecked('SoateEdit')} />
            </Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('SoateDelete')} checked={isChecked('SoateDelete')} />
            </Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>{localize('DataSourceQueues')}</Table.Cell>
            <Table.Cell>
              <Checkbox name="hidden" onChange={onChangeCreator('DataSourceQueuesView')} checked={isChecked('DataSourceQueuesView')} />
            </Table.Cell>
            <Table.Cell />
            <Table.Cell />
            <Table.Cell />
          </Table.Row>
        </Table.Body>
      </Table>
    </div>
  )
}

export default wrapper(FunctionalAttributes)
