import React, { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import debounce from "lodash.debounce";
import { 
  BackButton,
  HeaderBar,
  HeaderInput,
  Subtitle,
  Title,
  TitleWrapper,
} from "./Header.styled";

interface HeaderProps {
  searchTerm?: string;
  onSearchTermChanged?: (e: any) => void;
  onInputComplete?: (e: any) => void;
  display: 'search' | 'title_and_back';
  title?: string;
  subtitle?: string;
}

export const Header = (props: HeaderProps) => {
  const { display, title, subtitle, searchTerm, onSearchTermChanged, onInputComplete } = props;
  const navigate = useNavigate();
  
  const goBack = () => {
    navigate(-1);
  }

  const inputCompleteHandler = (e: any) => {
    onInputComplete && onInputComplete(e);
  };

  const debouncedInputCompleteHandler = useCallback(debounce(inputCompleteHandler, 225), []);

  return (
    <HeaderBar>
      {display === 'search' && <HeaderInput 
        type="text"
        value={searchTerm}
        onKeyDown={debouncedInputCompleteHandler}
        onChange={onSearchTermChanged}
        placeholder="Start typing a movie title..."/> 
      }
      {display === 'title_and_back' && <div>
        <BackButton onClick={() => goBack()}>Back</BackButton>
        {title && <TitleWrapper>
          <Title>{title}</Title>
          {subtitle && <Subtitle>{subtitle}</Subtitle>}
        </TitleWrapper>
        }
      </div>}
    </HeaderBar>
  );
};
