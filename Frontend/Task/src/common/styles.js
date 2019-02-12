import {Row} from 'antd'
import styled from 'styled-components'
import {ifProp} from 'styled-tools'

export const HeighRow = styled(Row)`
	${ifProp({type: `flex`}, `flex: auto;`)}
`