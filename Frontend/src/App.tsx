import { Layout } from 'components/Layout'
import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import { useTranslation } from 'react-i18next'
import { useThunkDispatch } from 'hooks/useThunkDispatch'
import { hydrateConfiguration } from 'state/actions/configuration'
import './i18n'

const App: React.FC = () => {
  const { t } = useTranslation()
  const dispatch = useThunkDispatch()

  useEffect(() => {
    dispatch(hydrateConfiguration())
  }, [dispatch])

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
