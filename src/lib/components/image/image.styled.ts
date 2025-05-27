import styled from 'styled-components';
import { ImgProps } from './image';

interface ImgPropsStyleTypes {
  $objectFit?: ImgProps['objectFit'];
}

export const StyledImg = styled.img<ImgPropsStyleTypes>`
  width: 100%;
  height: 100%;
  object-fit: ${(props: ImgPropsStyleTypes) => props?.$objectFit ?? 'cover'};
`;
