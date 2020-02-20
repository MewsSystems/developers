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
      <div>App</div>
    </>
  )
}

export default App
