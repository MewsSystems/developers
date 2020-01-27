import React from 'react'
import Head from '../components/Head/Head'
import { Button } from '../components/Button/Button'
import { Col, Container, Row } from '../utils/layout/grid.layout'

const Home = () => (
    <Container>
        <Head url={''} ogImage={''} title="Home" description={''} />
        <Row>
            <Col size={12}>
                <div style={{ textAlign: 'center' }}>
                    <h1>Mewies App</h1>
                    <Button onClick={() => alert('Hello!')}>Click</Button>
                </div>
            </Col>
        </Row>
    </Container>
)

export default Home
