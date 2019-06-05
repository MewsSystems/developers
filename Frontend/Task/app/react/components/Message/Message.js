import React, {PureComponent} from 'react';
import { connect } from 'react-redux';

class Message extends PureComponent {
  render() {
    const { requestError } = this.props;
    return (
      <>
        {
          requestError &&
          <div className='message__error'>
            Server internal error<br/>
            Data will be updated after some period of time
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
