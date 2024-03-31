import React from 'react';
import ReactDOM from 'react-dom/client';
import { Provider } from 'inversify-react';
import 'reflect-metadata';
import { container } from './ioc-container';
import { App } from './client/app';

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);
root.render(
    <React.StrictMode>
        <Provider container={container}>
            <App/>
        </Provider>
    </React.StrictMode>
);
