// App.js
import React from 'react'
import { hot } from 'react-hot-loader'

import { Button } from 'mews-ui';
import OrderDialog from './billings/OrderDialog';

const App = () => (
    <div>
        <OrderDialog />
    </div>
);

export default hot(module)(App);
