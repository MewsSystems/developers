import React, {PureComponent} from 'react';
import { connect } from 'react-redux';

class Message extends PureComponent {
  render() {
    const { requestError } = this.props;
    return (
      <>
        {
          requestError &&
          <div className='message message__error'>
            Oops! Error!<br/>
            Something went wrong, but don't worry!<br />
            The data will be updated after some period of time.
          </div>
        }
      </>
    );
  }
}

const mapStateToProps = state => ({
  requestError: state.currencies.requestError,
});

export default connect(mapStateToProps, null)(Message);
