import React from 'react'
import { RouteComponentProps } from '@reach/router'
import { SceneMetaProps, SceneMeta } from 'components/SceneMeta'
import { Heading } from 'components/Heading'
import { Spacing } from 'components/Spacing'

export const PageNotFound: React.FC<RouteComponentProps<SceneMetaProps>> = ({
  title,
  description,
}) => (
  <>
    <SceneMeta title={title} description={description} />
    <Spacing outer="2rem">
      <Heading level={1}>Page not found</Heading>
    </Spacing>
  </>
)
