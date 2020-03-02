import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Provider } from 'react-redux';
import { Normalize } from 'styled-normalize'
import { createGlobalStyle } from 'styled-components'
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from 'react-router-dom';

import store from 'domains/store';
import { Dispatch } from 'domains/reducers';
import Search from 'screens/Search';
import Detail from 'screens/Detail';
import { fetchConfig } from 'domains/ducks/config';
import constants from 'cssConstants';

const GlobalStyle = createGlobalStyle`
  body {
    background-color: ${constants.WHITE};
  }
`;

const mapDispatchToProps = (dispatch: Dispatch) => (bindActionCreators({
  fetchConfig,
}, dispatch));

type Props = ReturnType<typeof mapDispatchToProps>;

class AppWithConfig extends React.PureComponent<Props> {
  componentDidMount() {
    this.props.fetchConfig();
  }

  render() {
    return (
      <Router>
        <Switch>
          <Route path="/" exact component={Search}/>
          <Route
            path="/movie/:assetId"
            exact
            render={({ match }) => (
              <Detail key={match.params.assetId} assetId={match.params.assetId} />
            )}
          />
          <Redirect to="/" />
        </Switch>
        <Normalize />
        <GlobalStyle />
      </Router>
    );
  }
};

const ConnectedApp = connect(null, mapDispatchToProps)(AppWithConfig);

const App = () => {
  return (
    <Provider store={store}>
      <ConnectedApp/>
    </Provider>
  );
};
export default App;
