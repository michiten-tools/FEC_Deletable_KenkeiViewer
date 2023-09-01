select 
hex(FEC_DeviceLog.DeviceLogId),
FEC_FacilitySpec.SerialNumber, 
FEC_DeviceLog.Action, 
FEC_DeviceLog.TimeStampType,
FEC_DeviceLog.TimeStampStart,
FEC_DeviceLog.TimeStampEnd,
FEC_DamageLog.InspectionPoint,
FEC_DamageLog.LineNumber
from FEC_DeviceLog LEFT OUTER JOIN FEC_FacilitySpec on FEC_DeviceLog.FacilitySpecId =FEC_FacilitySpec.FacilitySpecId
                   LEFT OUTER JOIN FEC_Inspection on FEC_DeviceLog.InspectionId =FEC_Inspection.InspectionId
                   LEFT OUTER JOIN FEC_DamageLog on FEC_DeviceLog.InspectionId = FEC_DamageLog.InspectionId AND FEC_DeviceLog.LineNumber = FEC_DamageLog.LineNumber
where 
--FEC_FacilitySpec.SerialNumber = 'T0231610照明'
FEC_FacilitySpec.SerialNumber = 'T0231650照明'