import { Divider, Link, Stack, Tooltip } from '@mui/material';
import { t } from 'i18next';
import { PRIVACY_POLICY_LINK, TERMS_OF_USE_LINK } from '../../../../configs/config';

export default function Footer() {
  return (
    <Stack
      m={4}
      left={0}
      bottom={0}
      width="100%"
      position="fixed"
      justifyContent="center"
      direction="row"
      divider={<Divider orientation="vertical" flexItem />}
      spacing={2}>
      <Tooltip title={TERMS_OF_USE_LINK} arrow>
        <Link variant="body1" href={TERMS_OF_USE_LINK}>
          {t('common.termsOfUse')}
        </Link>
      </Tooltip>
      <Tooltip title={PRIVACY_POLICY_LINK} arrow>
        <Link variant="body1" href={PRIVACY_POLICY_LINK}>
          {t('common.privacyPolicy')}
        </Link>
      </Tooltip>
    </Stack>
  );
}
