import React from 'react'
import Helmet from 'react-helmet'

export interface SceneMetaProps {
  title: string
  description?: string
}

export const SceneMeta: React.FC<SceneMetaProps> = ({ title, description }) => (
  <Helmet>
    <title>{title}</title>
    {description && <meta name="description" content={description} />}
  </Helmet>
)
