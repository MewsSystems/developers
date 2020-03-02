import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import styled from 'styled-components'

import Image from 'components/Image';
import { fetchAsset, getAssetById } from 'domains/ducks/assets';
import { RootState, Dispatch } from 'domains/reducers';
import { getBaseUrl } from 'domains/ducks/config';
import constants from 'cssConstants';

type OwnProps = {
  assetId: string,
}

const mapStateToProps = (rootState: RootState, ownProps: OwnProps) => ({
  asset: getAssetById(rootState, ownProps.assetId),
  baseUrl: getBaseUrl(rootState),
});
const mapDispatchToProps = (dispatch: Dispatch) => (bindActionCreators({
  fetchAsset,
}, dispatch));

type Props = OwnProps & ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps>;

const Wrapper = styled.div`
  position: relative;
  display: flex;
  padding: 10vw;
  min-height: 100vh;
  box-sizing: border-box;
  background-color: black;
  @supports (backdrop-filter: blur(20px)) {
    background-color: ${constants.ACCENT};
  }
`;
const Background = styled.div`
  position: absolute;
  top: 0;
  left: 0;
  background-repeat: no-repeat;
  background-size: cover;
  background-position: center;
  width: 100%;
  height: 100%;
  opacity: 0.2;
  filter: blur(20px);
  z-index: 0;
`;
const Information = styled.div`
  position: relative;
  display: flex;
  flex-direction: column;
  margin-left: 5vw;
  z-index: 1;
  color: #F1F5FD;
`;
const StyledImage = styled(Image)`
  position: relative;
  flex : 0 0 20vw;
  height: 30vw;
  z-index: 1;
`;
const StyledImagePlaceholder = styled.div`
  flex: 0 0 20vw;
`;
const Title = styled.h1`
  font-size: 3rem;
  margin: 0;
  margin-bottom: 1rem;
  line-height: 1;
`;
const Tagline = styled.h2`
  font-size: 1rem;
  min-height: 1.15rem;
`;
const Genres = styled.div`
  display: flex;
  margin-top: 1rem;
`;
const GenreTag = styled.div`
  font-size: 1rem;
  border: 1px solid white;
  border-radius: 50px;
  padding: 1rem 1.5rem;
  margin-right: 1rem;
  background: rgba(0,0,0,0.35);
`;
const Overview = styled.p`
  line-height: 2;
`;

class Detail extends React.PureComponent<Props> {
  componentDidMount() {
    const { assetId } = this.props;

    // Search API returns a subset of detail API,
    // we need to fetch the asset again to have access to tagline, genres, etc...
    this.props.fetchAsset(assetId);
  }

  render() {
    const { asset, baseUrl } = this.props;

    if (!asset) { return null; }

    const {
      backdrop_path,
      genres,
      overview,
      poster_path,
      tagline,
      title,
    } = asset;

    const url = backdrop_path ? `${baseUrl}w1280${backdrop_path}` : null;

    return (
      <Wrapper>
        <Background style={{
          ...url ? { backgroundImage: `url(${url})` } : {},
        }}/>
        {poster_path ? (<StyledImage size={185} path={poster_path}/>) : <StyledImagePlaceholder/>}
        <Information>
          <Title>{title}</Title>
          <Tagline>{tagline}</Tagline>
          <Overview>{overview}</Overview>
          <Genres>
            {(genres || []).map((genre) => (
              <GenreTag key={genre.id}>{genre.name}</GenreTag>
            ))}
          </Genres>
        </Information>
      </Wrapper>
    );
  }
};

export default connect(mapStateToProps, mapDispatchToProps)(Detail);
