import React from 'react'
import App, { AppContext } from 'next/app'
import withRedux from 'next-redux-wrapper'
import { ApplicationState, initStore } from '../store/store'
import { Provider } from 'react-redux'
import { Store } from 'redux'
import { Colors } from '../utils/constants/color.constants'
import { createGlobalStyle } from 'styled-components'
import { Font } from '../utils/constants/font.constants'

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
                    <ResetCSS />
                    <Component {...pageProps} />
                </Provider>
            )
        }
    }
)

export const ResetCSS = createGlobalStyle`
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
        ::selection {
            background: ${Colors.yellow};
        }
        ::-moz-selection {
            background: ${Colors.yellow};
        }
    }
    body {
         font-family: ${Font.Montserrat};
         background: ${Colors.yellow};
    }
`
