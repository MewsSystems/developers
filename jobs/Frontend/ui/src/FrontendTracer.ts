import { ZoneContextManager } from "@opentelemetry/context-zone"
import {
  CompositePropagator,
  W3CBaggagePropagator,
  W3CTraceContextPropagator,
} from "@opentelemetry/core"
import { OTLPTraceExporter } from "@opentelemetry/exporter-trace-otlp-http"
import { registerInstrumentations } from "@opentelemetry/instrumentation"
import { FetchInstrumentation } from "@opentelemetry/instrumentation-fetch"
import { Resource, browserDetector } from "@opentelemetry/resources"
import { detectResourcesSync } from "@opentelemetry/resources/build/src/detect-resources"
import { BatchSpanProcessor } from "@opentelemetry/sdk-trace-base"
import { WebTracerProvider } from "@opentelemetry/sdk-trace-web"
import { SEMRESATTRS_SERVICE_NAME } from "@opentelemetry/semantic-conventions"

const FrontendTracer = () => {
  let resource = new Resource({
    [SEMRESATTRS_SERVICE_NAME]: "next-app-frontend",
  })

  const detectedResources = detectResourcesSync({
    detectors: [browserDetector],
  })
  resource = resource.merge(detectedResources)
  const provider = new WebTracerProvider({ resource })

  provider.addSpanProcessor(
    new BatchSpanProcessor(
      new OTLPTraceExporter({
        url: "http://localhost:8080/v1/traces",
      }),
      {
        scheduledDelayMillis: 500,
      },
    ),
  )

  const contextManager = new ZoneContextManager()

  provider.register({
    contextManager,
    propagator: new CompositePropagator({
      propagators: [
        new W3CBaggagePropagator(),
        new W3CTraceContextPropagator(),
      ],
    }),
  })

  registerInstrumentations({
    tracerProvider: provider,
    instrumentations: [
      new FetchInstrumentation({
        propagateTraceHeaderCorsUrls: /.*/,
        clearTimingResources: true,
      }),
    ],
  })
}

export default FrontendTracer
