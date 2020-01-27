import React from 'react'
import App, { AppContext } from 'next/app'
import withRedux from 'next-redux-wrapper'
import { ApplicationState, initStore } from '../store/store'
import { Provider } from 'react-redux'
import { Store } from 'redux'

interface Props {
    store: Store<ApplicationState>
}

export default withRedux(initStore)(
    class MyApp extends App<Props> {
        static async getInitialProps({ Component, ctx }: AppContext) {
            return {
                pageProps: {
                    ...(Component.getInitialProps
                        ? await Component.getInitialProps(ctx)
                        : {}),
                },
            }
        }

        render() {
            const { Component, pageProps, store } = this.props
            return (
                <Provider store={store}>
                    <Component {...pageProps} />
                </Provider>
            )
        }
    }
)
