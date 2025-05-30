import {LoadingContainer, CameraBody, SmallReel, FilmTape} from './LoadingCameraAnimation.styled';

export default function LoadingCameraAnimation() {
  return (
    <LoadingContainer>
      <CameraBody viewBox="0 0 200 200" fill="none" stroke="#555">
        <rect x="40" y="60" width="120" height="80" rx="8" strokeWidth="2" />
        <rect x="35" y="70" width="8" height="60" rx="2" strokeWidth="1.5" />
        <path d="M160 100 L175 90 V110 L160 100" strokeWidth="1.5" />
        <circle cx="160" cy="100" r="12" strokeWidth="1.5" />
        <circle cx="160" cy="100" r="8" strokeWidth="1" />
        <circle cx="160" cy="100" r="4" strokeWidth="1" />
        <FilmTape>
          <circle cx="70" cy="100" r="20" strokeWidth="1.5" />
          <circle cx="70" cy="100" r="16" strokeWidth="1" />
          <circle cx="70" cy="100" r="12" strokeWidth="1" />
          <path d="M70 85 L70 115 M55 100 L85 100" strokeWidth="1" />
          <circle cx="70" cy="100" r="3" strokeWidth="1" />
        </FilmTape>
        <SmallReel>
          <circle cx="130" cy="100" r="16" strokeWidth="1.5" />
          <circle cx="130" cy="100" r="12" strokeWidth="1" />
          <circle cx="130" cy="100" r="8" strokeWidth="1" />
          <path d="M130 88 L130 112 M118 100 L142 100" strokeWidth="1" />
          <circle cx="130" cy="100" r="2" strokeWidth="1" />
        </SmallReel>
        <rect x="60" y="50" width="80" height="8" rx="2" strokeWidth="1.5" />
        <circle cx="90" cy="54" r="2" strokeWidth="1" />
        <path d="M90 100 C90 100, 105 95, 115 100" strokeWidth="1" />
        <path d="M90 100 C90 100, 105 105, 115 100" strokeWidth="1" />
      </CameraBody>
    </LoadingContainer>
  );
}

LoadingCameraAnimation.displayName = 'LoadingCameraAnimation';
