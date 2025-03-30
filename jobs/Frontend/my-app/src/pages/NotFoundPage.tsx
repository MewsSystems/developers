import { BackToHPButton } from '../components/BackToHPButton';
import { NotFoundMessage } from '../components/NotFoundMessage';
import { PageSection } from '../components/PageSection';

export const NotFoundPage = () => {
  return (
    <PageSection>
      <BackToHPButton />
      <NotFoundMessage />
    </PageSection>
  );
};
