import { OTLPTraceExporter } from "@opentelemetry/exporter-trace-otlp-http"
import { Resource } from "@opentelemetry/resources"
import { NodeSDK } from "@opentelemetry/sdk-node"
import {
  BatchSpanProcessor,
  SpanProcessor,
} from "@opentelemetry/sdk-trace-node"
import { SEMRESATTRS_SERVICE_NAME } from "@opentelemetry/semantic-conventions"

export function getProcessor(): SpanProcessor {
  const exporter = new OTLPTraceExporter()

  return new BatchSpanProcessor(exporter)
}

const sdk = new NodeSDK({
  resource: new Resource({
    [SEMRESATTRS_SERVICE_NAME]: "next-app-backend",
  }),
  spanProcessors: [getProcessor()],
})
sdk.start()
