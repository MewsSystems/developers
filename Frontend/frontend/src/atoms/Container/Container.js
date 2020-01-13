import styled from 'styled-components';

import media from '../../utils/media';


/**
 * Container
 *  @fluid: false
 */
const Container = styled.div`
  width: 100%;
  padding-right: 0;
  padding-left: 0;
  margin-right: auto;
  margin-left: auto;
 

  ${(p) => !p.fluid && media.SM`
    max-width: 540px;`}
  ${(p) => !p.fluid && media.MD`
    max-width: 720px;`}
  ${(p) => !p.fluid && media.LG`
    max-width: 960px;`}
  ${(p) => !p.fluid && media.HD`
    max-width: 1140px;`}
  ${(p) => !p.fluid && media.HDPlus`
    max-width: 1400px;`}
  ${(p) => !p.fluid && media.FHD`
    max-width: 1650px;`}
  ${(p) => !p.fluid && media.QHD`
    max-width: 2200px;`}
  ${(p) => !p.fluid && media.QHDPlus`
    max-width: 2800px;`}
  ${(p) => !p.fluid && media.UHD`
    max-width: 3300px;`}
`;


Container.defaultProps = {
  fluid: false,
};


export default Container;
