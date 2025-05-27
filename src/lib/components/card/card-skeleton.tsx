import styled, { keyframes } from 'styled-components';

const shimmer = keyframes`
  0% {
    background-position: -200% 0;
  }
  100% {
    background-position: 200% 0;
  }
`;

const SkeletonBase = styled.div`
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 200% 100%;
  animation: ${shimmer} 1.5s infinite;
  border-radius: 4px;
`;

const CardSkeletonContainer = styled.div`
  height: 380px;
  width: 170px;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  overflow: hidden;
`;

const ImageSkeleton = styled(SkeletonBase)`
  width: 100%;
  height: 250px;
`;

const BodySkeleton = styled.div`
  padding: 4px 6px;
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 8px;
`;

const TitleSkeleton = styled(SkeletonBase)`
  height: 20px;
  width: 90%;
  margin-top: 20px;
`;

const FooterSkeleton = styled(SkeletonBase)`
  height: 16px;
  width: 80%;
  margin: 0 6px 6px;
`;

export const CardSkeleton = () => {
  return (
    <CardSkeletonContainer>
      <ImageSkeleton />
      <BodySkeleton>
        <TitleSkeleton />
      </BodySkeleton>
      <FooterSkeleton />
    </CardSkeletonContainer>
  );
};
