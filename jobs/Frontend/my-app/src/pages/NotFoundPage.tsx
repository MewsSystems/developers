import { BackButton } from '../components/BackButton';
import { NotFoundMessage } from '../components/NotFoundMessage';
import { PageSection } from '../components/PageSection';

export const NotFoundPage = () => {
  return (
    <PageSection>
      <BackButton />
      <NotFoundMessage />
    </PageSection>
  );
};
