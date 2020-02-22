import { Layout } from 'components/Layout'
import React from 'react'
import { Helmet } from 'react-helmet'
import { useTranslation } from 'react-i18next'
import './i18n'

const App: React.FC = () => {
  const { t } = useTranslation()

  return (
    <>
      <Helmet>
        <title>{t('html-title')}</title>
      </Helmet>
      <Layout>App</Layout>
    </>
  )
}

export default App
