# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: exchange_rates.proto
"""Generated protocol buffer code."""
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='exchange_rates.proto',
  package='',
  syntax='proto3',
  serialized_options=None,
  create_key=_descriptor._internal_create_key,
  serialized_pb=b'\n\x14\x65xchange_rates.proto\"B\n\x14\x45xchangeRatesRequest\x12\x19\n\x0c\x63urrencyCode\x18\x01 \x01(\tH\x00\x88\x01\x01\x42\x0f\n\r_currencyCode\"x\n\x15\x45xchangeRatesResponse\x12\x14\n\x0c\x63urrencyCode\x18\x01 \x01(\t\x12\x0c\n\x04unit\x18\x02 \x01(\x05\x12\x10\n\x08\x63urrency\x18\x03 \x01(\t\x12\x13\n\x0b\x66orexBuying\x18\x04 \x01(\x01\x12\x14\n\x0c\x66orexSelling\x18\x05 \x01(\x01\x32R\n\rExchangeRates\x12\x41\n\x10GetExchangeRates\x12\x15.ExchangeRatesRequest\x1a\x16.ExchangeRatesResponseb\x06proto3'
)




_EXCHANGERATESREQUEST = _descriptor.Descriptor(
  name='ExchangeRatesRequest',
  full_name='ExchangeRatesRequest',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='currencyCode', full_name='ExchangeRatesRequest.currencyCode', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
    _descriptor.OneofDescriptor(
      name='_currencyCode', full_name='ExchangeRatesRequest._currencyCode',
      index=0, containing_type=None,
      create_key=_descriptor._internal_create_key,
    fields=[]),
  ],
  serialized_start=24,
  serialized_end=90,
)


_EXCHANGERATESRESPONSE = _descriptor.Descriptor(
  name='ExchangeRatesResponse',
  full_name='ExchangeRatesResponse',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='currencyCode', full_name='ExchangeRatesResponse.currencyCode', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='unit', full_name='ExchangeRatesResponse.unit', index=1,
      number=2, type=5, cpp_type=1, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='currency', full_name='ExchangeRatesResponse.currency', index=2,
      number=3, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='forexBuying', full_name='ExchangeRatesResponse.forexBuying', index=3,
      number=4, type=1, cpp_type=5, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='forexSelling', full_name='ExchangeRatesResponse.forexSelling', index=4,
      number=5, type=1, cpp_type=5, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=92,
  serialized_end=212,
)

_EXCHANGERATESREQUEST.oneofs_by_name['_currencyCode'].fields.append(
  _EXCHANGERATESREQUEST.fields_by_name['currencyCode'])
_EXCHANGERATESREQUEST.fields_by_name['currencyCode'].containing_oneof = _EXCHANGERATESREQUEST.oneofs_by_name['_currencyCode']
DESCRIPTOR.message_types_by_name['ExchangeRatesRequest'] = _EXCHANGERATESREQUEST
DESCRIPTOR.message_types_by_name['ExchangeRatesResponse'] = _EXCHANGERATESRESPONSE
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

ExchangeRatesRequest = _reflection.GeneratedProtocolMessageType('ExchangeRatesRequest', (_message.Message,), {
  'DESCRIPTOR' : _EXCHANGERATESREQUEST,
  '__module__' : 'exchange_rates_pb2'
  # @@protoc_insertion_point(class_scope:ExchangeRatesRequest)
  })
_sym_db.RegisterMessage(ExchangeRatesRequest)

ExchangeRatesResponse = _reflection.GeneratedProtocolMessageType('ExchangeRatesResponse', (_message.Message,), {
  'DESCRIPTOR' : _EXCHANGERATESRESPONSE,
  '__module__' : 'exchange_rates_pb2'
  # @@protoc_insertion_point(class_scope:ExchangeRatesResponse)
  })
_sym_db.RegisterMessage(ExchangeRatesResponse)



_EXCHANGERATES = _descriptor.ServiceDescriptor(
  name='ExchangeRates',
  full_name='ExchangeRates',
  file=DESCRIPTOR,
  index=0,
  serialized_options=None,
  create_key=_descriptor._internal_create_key,
  serialized_start=214,
  serialized_end=296,
  methods=[
  _descriptor.MethodDescriptor(
    name='GetExchangeRates',
    full_name='ExchangeRates.GetExchangeRates',
    index=0,
    containing_service=None,
    input_type=_EXCHANGERATESREQUEST,
    output_type=_EXCHANGERATESRESPONSE,
    serialized_options=None,
    create_key=_descriptor._internal_create_key,
  ),
])
_sym_db.RegisterServiceDescriptor(_EXCHANGERATES)

DESCRIPTOR.services_by_name['ExchangeRates'] = _EXCHANGERATES

# @@protoc_insertion_point(module_scope)
