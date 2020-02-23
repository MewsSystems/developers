import React from 'react'
import { RouteComponentProps } from '@reach/router'
import { SceneMetaProps, SceneMeta } from 'components/SceneMeta'
import { Heading } from 'components/Heading'
import { Content } from 'components/Content'

export const PageNotFound: React.FC<RouteComponentProps<SceneMetaProps>> = ({
  title,
  description,
}) => (
  <>
    <SceneMeta title={title} description={description} />
    <Content>
      <Heading level={1}>Page not found</Heading>
    </Content>
  </>
)
