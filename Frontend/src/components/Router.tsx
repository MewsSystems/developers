import React from 'react'
import { Router as ReachRouter } from '@reach/router'
import { Home } from 'scenes/Home'
import { useTranslation } from 'react-i18next'
import { PageNotFound } from 'scenes/PageNotFound'
import { Detail } from 'scenes/Detail'

const { PUBLIC_URL } = process.env

export const Router: React.FC = () => {
  const { t } = useTranslation('scenes')

  return (
    <ReachRouter>
      <Home
        path={`${PUBLIC_URL}/`}
        title={t('home.title')}
        description={t('home.desciption')}
      />
      <Detail path={`${PUBLIC_URL}/movie/:id`} />
      <PageNotFound path={`${PUBLIC_URL}/*`} />
    </ReachRouter>
  )
}
