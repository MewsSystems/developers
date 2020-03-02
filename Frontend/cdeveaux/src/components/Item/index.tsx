import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import styled from 'styled-components'

import Image from 'components/Image';
import { RootState } from 'domains/reducers';
import { getAssetById } from 'domains/ducks/assets';

type OwnProps = {
  id: string,
  withOverlay?: boolean,
}

const mapStateToProps = (rootState: RootState, ownProps: OwnProps) => ({
  asset: getAssetById(rootState, ownProps.id),
});

type Props = OwnProps & ReturnType<typeof mapStateToProps>;

const StyledLink = styled(Link)`
  position: relative;
  flex: 0 0 15vw;
  height: 22vw;
  margin: 2vw;
  border-radius: 7px;
  overflow: hidden;
  box-shadow: 13px 13px 14px -10px rgba(0,0,0,0.64);
  &:hover:after {
    opacity: 1;
  }
  &:after {
    position: absolute;
    content: '';
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    box-shadow: 25px 25px 36px -20px rgba(0,0,0,0.64);
    transition: opacity 120ms ease-in-out;
    opacity: 0;
  }
`;
const Overlay = styled.div`
  position: absolute;
  display: flex;
  box-sizing: border-box;
  background: rgba(0,0,0,0.75);
  bottom: 0;
  height: 100%;
  width: 100%;
  justify-content: center;
  align-items: center;
  color: white;
  opacity: 0;
  padding: 1.5rem;
  transition: opacity 120ms ease-in-out;
  ${StyledLink}:hover & {
    opacity: 1;
  }
  @supports (backdrop-filter: blur(6px)) {
    background: rgba(0,0,0,0.6);
    backdrop-filter: blur(10px);
  }
`;

class Item extends React.PureComponent<Props> {
  static defaultProps = {
    withOverlay: true,

  };

  render() {
    const {
      asset,
    } = this.props;

    const {
      title,
      poster_path,
    } = asset;

    return (
      <StyledLink to={`/movie/${asset.id}`}>
        <Image size={185} path={poster_path}/>
        <Overlay>
          {title}
        </Overlay>
        <br/>
      </StyledLink>
    );
  }
}

export default connect(mapStateToProps)(Item);
