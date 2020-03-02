import React from 'react';
import { connect } from 'react-redux';
import styled from 'styled-components'

import { getBaseUrl } from 'domains/ducks/config';
import { RootState } from 'domains/reducers';
import constants from 'cssConstants';

type OwnProps = {
  path: string,
  size: number,
  className?: string,
};
const mapStateToProps = (rootState: RootState, ownProps: OwnProps) => ({
  baseUrl: getBaseUrl(rootState),
});

type Props = typeof Image.defaultProps & OwnProps & ReturnType<typeof mapStateToProps>;

const Background = styled.div`
  background-size: cover;
  width: 100%;
  height: 100%;
  background-position: center;
  background-color: ${constants.ACCENT};
`;

class Image extends React.PureComponent<Props> {
  static defaultProps = {
    size: 185,
  };

  render() {
    const { baseUrl, size, path, className } = this.props;
    const url = path ? `${baseUrl}w${size}${path}` : null;

    return (<Background className={className} style={{
      ...url ? { backgroundImage: `url(${url})` } : {},
    }}/>);
  }
};

export default connect(mapStateToProps)(Image);
