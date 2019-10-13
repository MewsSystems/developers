import React, { Component } from 'react';
import Axios from 'axios';

export default class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      apiConfiguration: [],
    };
    this.apiConfigurationDownload();
  }

  apiConfigurationDownload() {
    console.log('run');
    Axios.get('http://localhost:3000/configuration', { timeout: 10000 }).then(
      response => {
        const apiConfiguration = response.data;
        console.log('RUN', apiConfiguration);
        this.setState({
          apiConfiguration,
        });
      }
    );
  }

  // // text for the component search function
  // onTextChange(text) {
  //     this.setState({searchText : text});
  // };

  // render contains the navbar and the other component.

  render() {
    return (
      <div className="main">
        <nav className="navbar navbar-light">
          <h5 className="navbar-brand mx-auto mb-2" href="">
            Currency Converter
          </h5>
          <div className="col-md-12 app-list p-0">
            <ul className="list-group">
              <li className="list-group-item">A</li>
              <li className="list-group-item">B</li>
              <li className="list-group-item">C</li>
              <li className="list-group-item">D</li>
            </ul>
          </div>
        </nav>
      </div>
    );
  }
}
