import React from 'react'
import { Route, IndexRoute } from 'react-router'

import List from './List'

const Layout = props => <div>{props.children}</div>

export default (
  <Route path="soates" component={Layout}>
    <IndexRoute component={List} />
  </Route>
)