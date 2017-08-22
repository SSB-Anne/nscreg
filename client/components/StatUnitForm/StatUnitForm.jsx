import React from 'react'
import { shape, func, node } from 'prop-types'
import { Icon } from 'semantic-ui-react'

import Form from 'components/Form'
import styles from './styles.pcss'

const StatUnitForm = ({ formData, schema, localize, onChange, onSubmit, children, onCancel }) => (
  <Form
    value={formData}
    onChange={onChange}
    onSubmit={onSubmit}
    schema={schema}
    className={styles['statunit-form']}
  >
    {children}
    <br key="stat_unit_form_br" />
    <Form.Button
      content={localize('Back')}
      onClick={onCancel}
      floated="left"
      icon={<Icon size="large" name="chevron left" />}
      size="small"
      color="grey"
      type="button"
      key="stat_unit_form_back_btn"
    />
    <Form.Button
      content={localize('Submit')}
      key="stat_unit_form_submit_btn"
      type="submit"
      floated="right"
      primary
    />
  </Form>
)

StatUnitForm.propTypes = {
  children: node.isRequired,
  formData: shape({}).isRequired,
  schema: shape({}).isRequired,
  localize: func.isRequired,
  onChange: func.isRequired,
  onSubmit: func.isRequired,
  onCancel: func.isRequired,
}

export default StatUnitForm
