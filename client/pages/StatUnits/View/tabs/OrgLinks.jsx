import React from 'react'
import { func, oneOfType, number, string } from 'prop-types'
import Tree from 'antd/lib/tree'
import { Segment, Loader, Header } from 'semantic-ui-react'

import styles from './styles.pcss'

const hasChildren = node => node.orgLinksNodes && node.orgLinksNodes.length > 0
const TreeNode = Tree.TreeNode

class OrgLinks extends React.Component {
  static propTypes = {
    id: oneOfType([number, string]).isRequired,
    fetchData: func.isRequired,
    activeTab: string.isRequired,
    localize: func.isRequired,
  }

  state = { orgLinksRoot: undefined }

  componentDidMount() {
    const { id, fetchData } = this.props
    fetchData({ id }).then(orgLinksRoot => this.setState({ orgLinksRoot }))
  }

  renderChildren(nodes) {
    return nodes.map((node) => {
      const anyChild = hasChildren(node)
      return (
        <TreeNode uid={node.regId} key={node.regId} title={node.name} isLeaf={!anyChild}>
          {anyChild && this.renderChildren(node.orgLinksNodes)}
        </TreeNode>
      )
    })
  }

  render() {
    const { orgLinksRoot } = this.state
    const { activeTab, localize } = this.props
    const highLight = node => node.props.uid === this.props.id
    return (
      <div>
        {activeTab !== 'orgLinks' && (
          <Header as="h5" className={styles.heigthHeader} content={localize('OrgLinks')} />
        )}
        <Segment>
          {orgLinksRoot ? (
            <Tree filterTreeNode={highLight} defaultExpandAll>
              <TreeNode uid={orgLinksRoot.regId} title={orgLinksRoot.name} key={orgLinksRoot.regId}>
                {hasChildren(orgLinksRoot) && this.renderChildren(orgLinksRoot.orgLinksNodes)}
              </TreeNode>
            </Tree>
          ) : (
            <Loader active />
          )}
        </Segment>
      </div>
    )
  }
}

export default OrgLinks
